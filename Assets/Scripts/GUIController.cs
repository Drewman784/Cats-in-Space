using System;
using TbsFramework.Cells;
using TbsFramework.Example4;
using TbsFramework.Grid;
using TbsFramework.Grid.GridStates;
using TbsFramework.Players;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TbsFramework.Gui
{
    public class GUIController : MonoBehaviour
    {
        public CellGrid CellGrid;
        public Button EndTurnButton;
        public Button RestartButton;
        public Button MainMenuButton;

        public GameObject TerrainPanel;
        public Text TerrainNameText;
        public Text MovementCostText;
        public Text DefenceBoostText;

        void Awake()
        {
            CellGrid.LevelLoading += OnLevelLoading;
            CellGrid.LevelLoadingDone += OnLevelLoadingDone;
            CellGrid.GameEnded += OnGameEnded;
            CellGrid.TurnEnded += OnTurnEnded;
            CellGrid.GameStarted += OnGameStarted;

            TerrainPanel.SetActive(false);
        }

        private void OnGameStarted(object sender, EventArgs e)
        {
            foreach (Transform cell in CellGrid.transform)
            {
                cell.GetComponent<Cell>().CellHighlighted += OnCellHighlighted;
                cell.GetComponent<Cell>().CellDehighlighted += OnCellDehighlighted;
            }


            if (EndTurnButton != null)
            {
                EndTurnButton.interactable = CellGrid.CurrentPlayer is HumanPlayer;
            }
        }

        private void OnCellDehighlighted(object sender, EventArgs e)
        {
            TerrainPanel.SetActive(false);
        }

        private void OnTurnEnded(object sender, bool isNetworkInvoked)
        {
            if (EndTurnButton != null)
            {
                EndTurnButton.interactable = CellGrid.CurrentPlayer is HumanPlayer;
            }
        }

        private void OnCellHighlighted(object sender, EventArgs e)
        {
            var cell = sender as Tile_Script;
            TerrainNameText.text = cell.TileType;
            MovementCostText.text = string.Format("Mov cost: {0}", cell.MovementCost);
            DefenceBoostText.text = string.Format("Def boost: {0}", cell.DefenseBoost);

            TerrainPanel.SetActive(true);
        }

        private void OnGameEnded(object sender, GameEndedArgs e)
        {
            Debug.Log(string.Format("Player{0} wins!", e.gameResult.WinningPlayers[0]));
            /*if (EndTurnButton != null)
            {
                EndTurnButton.interactable = false;
            }*/
        }

        private void OnLevelLoading(object sender, EventArgs e)
        {
            Debug.Log("Level is loading");
        }

        private void OnLevelLoadingDone(object sender, EventArgs e)
        {
            Debug.Log("Level loading done");
            Debug.Log("Press 'm' to end turn");
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.M) && !(CellGrid.cellGridState is CellGridStateAITurn))
            {
                EndTurn();//User ends his turn by pressing "m" on keyboard.
            }
        }

        public void EndTurn()
        {
            CellGrid.EndTurn();
        }
        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().path);
        }
        public void ExittoMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}