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
            var playerUnits = cellGrid.Units.Where(u => u.PlayerNumber == 0).ToList();
            var enemyUnits = cellGrid.Units.Where(u => u.PlayerNumber != 0).ToList();

            if (playerUnits.Count == 0)
            {
                // Player has lost all units, restart the current level
                //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                cellGrid.LoseScren();
                return new GameResult(true, null, null);
            }
            else if (enemyUnits.Count == 0)
            {
                // All enemy units are gone, load the next scene
                if (!string.IsNullOrEmpty(NextScene))
                {
                    //SceneManager.LoadScene(NextScene);
                    cellGrid.WinScreen(NextScene);
                }
                else
                {
                    //Debug.LogError("NextScene is not set. Cannot load the next scene.");
                }
                return new GameResult(true, null, null);
            }

            return new GameResult(false, null, null);
        }
    }
}

