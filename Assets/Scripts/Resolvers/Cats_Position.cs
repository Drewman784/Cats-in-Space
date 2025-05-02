using System.Collections.Generic;
using System.Linq;
using TbsFramework.Cells;
using TbsFramework.Units;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TbsFramework.Grid.GameResolvers
{

    public class Cats_Position : GameEndCondition
    {

        //public SceneAsset NextLevel;
        public string NextScene;

        //var playerUnits = cellGrid.Units.Where(u => u.PlayerNumber == 0).ToList();
        //var enemyUnits = cellGrid.Units.Where(u => u.PlayerNumber != 0).ToList();

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
                //SceneManager.LoadScene(NextScene);

                if (cellGrid.Units.All(u => u.PlayerNumber == AppliesToPlayerNo && u.HitPoints <= 0))
                {
                    // Reload the current scene if the player loses all their units
                    //SceneManager.LoadScene(SceneManager.GetActiveScene().name);

                    cellGrid.LoseScren();
                    return new GameResult(true, null, null);
                }
                else
                {
                    
                    // Advance to the next scene if the player reaches the DestinationCell
                    //SceneManager.LoadScene(NextScene);

                    if (!string.IsNullOrEmpty(NextScene))
                    {
                        //SceneManager.LoadScene(NextScene);
                        cellGrid.WinScreen(NextScene);
                    }
                    return new GameResult(true, null, null);

                    
                }
            } else{
                List<Unit> pUnits = cellGrid.GetPlayerUnits(cellGrid.Players[0]);
                    Debug.Log("player units: " + pUnits.Count);
                    if(pUnits.Count==0){
                         cellGrid.LoseScren();
                        return new GameResult(true, null, null);
                    }
            }
            return new GameResult(false, null, null);
        }
    }
}

