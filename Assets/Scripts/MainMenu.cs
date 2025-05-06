using System;
using TbsFramework.Grid;
using TbsFramework.Grid.GridStates;
using TbsFramework.Players;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TbsFramework.Gui
{
    public class MainMenu : MonoBehaviour
    {
        public Button ExitButton;
        public Button NewGameButton;
        public Button LoadGameButton;
        public Button SkirmishButton;
        public Button OptionsButton;
        public Button CreditButton;

        [SerializeField] GameObject CreditsPage;

        //public Button TempLevelTwo;
        //public Button TempLevelThree;

        public void Skirmish()
        {
            SceneManager.LoadScene("TestingScene");
        }
        public void NewGame()
        {
            SceneManager.LoadScene("Level_One");
        }
        //public void Temp_LevelTwo()
        //{
        //    SceneManager.LoadScene("Level_Two");
        //}
        //public void Temp_LevelThree()
        //{
        //    SceneManager.LoadScene("Level_Three");
        //}
        public void ExitGame()
        {
            Application.Quit();
        }

        void Awake()
        {
            
        }

        public void OpenCredits(){
            CreditsPage.SetActive(true);
        }

        public void CloseCredits(){
            CreditsPage.SetActive(false);
        }

        //private void OnLevelLoading(object sender, EventArgs e)
        //{
        //    Debug.Log("Level is loading");
        //}
        //
        //private void OnLevelLoadingDone(object sender, EventArgs e)
        //{
        //    Debug.Log("Level loading done");
        //    Debug.Log("Press 'm' to end turn");
        //}

        void Update()
        {

        }
    }
}