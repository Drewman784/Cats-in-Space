using System.Collections.Generic;
using System.Linq;
using TbsFramework.Cells;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TbsFramework.Grid.GameResolvers
{

    public class PositionCondition : GameEndCondition
    {

        //public SceneAsset NextLevel;
        public string NextScene;

        public Cell DestinationCell;
        [Tooltip("Specifies whether the condition applies to any player")]
        public bool AnyPlayer;
        [Tooltip("Specifies a single player that the condition applies to")]
        public int AppliesToPlayerNo;

        public override GameResult CheckCondition(CellGrid cellGrid)
        {
            if (DestinationCell.CurrentUnits.Count > 0
                && (DestinationCell.CurrentUnits.Exists(u => u.PlayerNumber == AppliesToPlayerNo) || AnyPlayer == true))
            {
                var winningPlayers = new List<int>() { DestinationCell.CurrentUnits[0].PlayerNumber };
                var loosingPlayers = cellGrid.Players.Where(p => p.PlayerNumber != DestinationCell.CurrentUnits[0].PlayerNumber)
                                                     .Select(p => p.PlayerNumber)
                                                     .ToList();

                // Change the scene to the one listed in the inspector
                SceneManager.LoadScene(NextScene);

                return new GameResult(true, winningPlayers, loosingPlayers);
            }

            return new GameResult(false, null, null);
        }
    }
}

