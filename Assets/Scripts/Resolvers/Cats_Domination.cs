using System.Linq;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace TbsFramework.Grid.GameResolvers
{
    public class Cats_Domination : GameEndCondition
    {
        //public SceneAsset NextLevel;
        public string NextScene;

        public override GameResult CheckCondition(CellGrid cellGrid)
        {
            var playersAlive = cellGrid.Units.Select(u => u.PlayerNumber).Distinct().ToList();
            if (playersAlive.Count == 1)
            {
                var playersDead = cellGrid.Players.Where(p => p.PlayerNumber != playersAlive[0])
                                                  .Select(p => p.PlayerNumber)
                                                  .ToList();

                // Change the scene to the one listed in the inspector
                SceneManager.LoadScene(NextScene);

                return new GameResult(true, playersAlive, playersDead);
            }
            return new GameResult(false, null, null);
        }
    }
}

