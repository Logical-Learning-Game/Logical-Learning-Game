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
        [SerializeField] LoadingScreen loadingScreen;
        [SerializeField] PopupScreen popupScreen;

        [Header("Scenes")]
        [SerializeField] string MainMenuSceneName = "MainMenu";
        [SerializeField] string GameSceneName = "GameMode";




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
            NewGameScreen.GoogleNewGameClick += ShowGoogleSyncScreenFromNewGame;
            //NewGameScreen.LocalNewGameClick += ShowPanelScreen;

            GoogleSyncScreen.CancelSyncClick += OnCancelSyncClick;
            SettingPanelManager.GoogleSyncClick += ShowGoogleSyncScreenFromSetting;

            MapEntryManager.SelectMap += LoadGameScene;

            GameDataManager.NewGameCompleted += ShowPanelScreen;

            SettingPanelManager.MainMenuClick += ReloadScene;
            SettingPanelManager.HowToPlayClick += OnOpenPopupScreen;
            PopupScreen.CloseModalClick += OnCloseTutorial;
        }

        private void OnDisable()
        {
            IntroScreen.NewGameClick -= ShowNewGameScreen;
            IntroScreen.ContinueClick -= ShowPanelScreen;

            NewGameScreen.BackClick -= ShowIntroScreen;
            NewGameScreen.GoogleNewGameClick -= ShowGoogleSyncScreenFromNewGame;
            SettingPanelManager.GoogleSyncClick -= ShowGoogleSyncScreenFromSetting;

            GoogleSyncScreen.CancelSyncClick -= OnCancelSyncClick;

            MapEntryManager.SelectMap -= LoadGameScene;

            GameDataManager.NewGameCompleted -= ShowPanelScreen;

            SettingPanelManager.MainMenuClick -= ReloadScene;
            SettingPanelManager.HowToPlayClick -= OnOpenPopupScreen;
            PopupScreen.CloseModalClick -= OnCloseTutorial;
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

            if (panelScreen != null)
                allModalScreens.Add(panelScreen);

            if (loadingScreen != null)
                allModalScreens.Add(loadingScreen);

            if (popupScreen != null)
                allModalScreens.Add(popupScreen);
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

        public void ShowGoogleSyncScreenFromNewGame()
        {
            ShowModalScreen(googleSyncScreen);
            googleSyncScreen.LatestScreen = "NewGame";
        }

        public void ShowGoogleSyncScreenFromSetting()
        {
            ShowModalScreen(googleSyncScreen);
            googleSyncScreen.LatestScreen = "SettingPanel";
        }

        public void ShowPanelScreen()
        {
            ShowModalScreen(panelScreen);
            panelScreen.ShowLevelPanel(null);
        }

        public void LoadGameScene(bool isSameMap)
        {
            AudioManager.PlayLevelStartSound();
            Time.timeScale = 1f;
#if UNITY_EDITOR
            if (Application.isPlaying)
            {
#endif
                ShowModalScreen(loadingScreen);
                SceneManager.LoadSceneAsync(GameSceneName);
#if UNITY_EDITOR
            }
#endif

        }

        public void ReloadScene()
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
#endif
                SceneManager.LoadSceneAsync(MainMenuSceneName);
        }

        public void OnOpenPopupScreen(int contentIndex)
        {
            ShowModalScreen(popupScreen);
            popupScreen.ShowContent();
        }

        public void OnCloseTutorial()
        {
            ShowModalScreen(panelScreen);
            panelScreen.ShowSettingPanel(null);
        }

        public void OnCancelSyncClick(string latestScreen)
        {
            if(latestScreen == "NewGame")
            {
                ShowNewGameScreen();
            }
            else if (latestScreen == "SettingPanel")
            {
                ShowModalScreen(panelScreen);
                panelScreen.ShowSettingPanel(null);
            }
            else
            {
                ShowIntroScreen();
            }
        }
    }
}

