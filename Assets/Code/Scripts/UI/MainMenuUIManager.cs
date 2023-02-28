using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Unity.Game.SaveSystem;
using Unity.Game.Level;


namespace Unity.Game.UI
{
    public class MainMenuUIManager : MonoBehaviour
    {

        [Header("Modal Menu Screens")]
        [Tooltip("Only one modal interface can appear on-screen at a time.")]
        [SerializeField] IntroScreen introScreen;
        [SerializeField] NewGameScreen newGameScreen;
        [SerializeField] GoogleSyncScreen googleSyncScreen;
        [SerializeField] PanelScreen panelScreen;

        [Header("Scenes")]
        [SerializeField] string MainMenuSceneName = "MainMenu";
        [SerializeField] string GameSceneName = "GameMode";

        public static event Action OpenPanel;


        //[Header("Full-screen overlays")]
        //[Tooltip("Full-screen overlays block other controls until dismissed.")]
        //[SerializeField] MenuScreen m_InventoryScreen;
        //[SerializeField] SettingsScreen m_SettingsScreen;

        List<MenuScreen> allModalScreens = new List<MenuScreen>();
        UIDocument mainMenuDocument;
        
        public UIDocument MainMenuDocument => mainMenuDocument;

        void OnEnable()
        {
            mainMenuDocument = GetComponent<UIDocument>();
            SetupModalScreens();
            ShowIntroScreen();

            IntroScreen.NewGameClick += ShowNewGameScreen;
            IntroScreen.ContinueClick += ShowPanelScreen;

            NewGameScreen.BackClick += ShowIntroScreen;
            NewGameScreen.GoogleNewGameClick += ShowGoogleSyncScreen;
            //NewGameScreen.LocalNewGameClick += ShowPanelScreen;

            GoogleSyncScreen.CancelSyncClick += ShowNewGameScreen;

            MapEntryManager.SelectMap += LoadGameScene;

            GameDataManager.NewGameCompleted += ShowPanelScreen;
        }

        private void OnDisable()
        {
            IntroScreen.NewGameClick -= ShowNewGameScreen;
            IntroScreen.ContinueClick -= ShowPanelScreen;

            NewGameScreen.BackClick -= ShowIntroScreen;
            NewGameScreen.GoogleNewGameClick -= ShowGoogleSyncScreen;
            //NewGameScreen.LocalNewGameClick -= ShowPanelScreen;

            GoogleSyncScreen.CancelSyncClick -= ShowNewGameScreen;

            MapEntryManager.SelectMap -= LoadGameScene;

            GameDataManager.NewGameCompleted -= ShowPanelScreen;

        }

        void Start()
        {
            Time.timeScale = 1f;
        }

        void SetupModalScreens()
        {
            if (introScreen != null)
                allModalScreens.Add(introScreen);

            if (newGameScreen != null)
                allModalScreens.Add(newGameScreen);

            if (googleSyncScreen != null)
                allModalScreens.Add(googleSyncScreen);

            if(panelScreen != null)
                allModalScreens.Add(panelScreen);
        }

        // shows one screen at a time
        void ShowModalScreen(MenuScreen modalScreen)
        {
            foreach (MenuScreen m in allModalScreens)
            {
                if (m == modalScreen)
                {

                    m?.ShowScreen();
                }
                else
                {

                    m?.HideScreen();
                }
            }
        }

        // methods to toggle screens on/off

        // modal screen methods 
        public void ShowIntroScreen()
        {
            ShowModalScreen(introScreen);
        }

        public void ShowNewGameScreen()
        {
            ShowModalScreen(newGameScreen);
        }
        
        public void ShowGoogleSyncScreen()
        {
            ShowModalScreen(googleSyncScreen);
        }

        public void ShowPanelScreen()
        {
            OpenPanel?.Invoke();
            ShowModalScreen(panelScreen);
        }

        public void LoadGameScene()
        {
            Time.timeScale = 1f;
#if UNITY_EDITOR
            if (Application.isPlaying)

#endif
                SceneManager.LoadSceneAsync(GameSceneName);
        }
    }
}

