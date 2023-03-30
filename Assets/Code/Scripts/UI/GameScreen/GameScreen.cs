using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;
using Unity.Game.Command;
using System.Linq;
using Unity.Game.SaveSystem;

namespace Unity.Game.UI
{
    public class GameScreen : MonoBehaviour
    {

        public static event Action<float> GamePaused;
        public static event Action GameResumed;
        public static event Action GameQuit;
        public static event Action<bool> GameRestarted;
        public static event Action<float> MusicVolumeChanged;
        public static event Action<float> SfxVolumeChanged;

        [Header("Blur")]
        [SerializeField] Volume Volume;

        [SerializeField] GameObject DefaultInGameScreen;

        List<MenuScreen> allModalScreens = new List<MenuScreen>();
        [SerializeField] PanelScreen panelScreen;
        [SerializeField] VictoryScreen victoryScreen;
        [SerializeField] LoadingScreen loadingScreen;
        [SerializeField] GoogleSyncScreen googleSyncScreen;
        [SerializeField] PopupScreen popupScreen;

        [SerializeField] Button OutsidePanel;

        [SerializeField] private bool IsGameWinning;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if(DefaultInGameScreen.transform.localScale != Vector3.zero)
                {
                    OnOpenJournalMenu();
                }
                else
                {
                    OnOpenGameScreen(null);
                }
                AudioManager.PlayDefaultButtonSound();
            }
        }
        void OnEnable()
        {
            SetVisualElements();
            RegisterButtonCallbacks();
            SetupModalScreens();

            if (Volume == null)
                Volume = FindObjectOfType<Volume>();

            GameScreenController.GameWon += OnGameWon;
            MapEntryManager.SelectMap += OnSelectMap;
            VictoryScreen.RestartClick += OnPlayerRestarted;
            VictoryScreen.SelectMapClick += OnOpenJournalMenu;
            SettingPanelManager.MainMenuClick += OnGameQuit;
            SettingPanelManager.GoogleSyncClick += OnClickSync;
            SettingPanelManager.HowToPlayClick += OnOpenPopupScreen;
            GoogleSyncScreen.CancelSyncClick += OnCancelSyncClick;
            GameDataManager.NewGameCompleted += OnOpenJournalMenu;
            PopupScreen.CloseModalClick += OnClosePopupScreen;
            GameScreenController.ShowTutorial += OnOpenPopupScreen;


        }

        void OnDisable()
        {
            GameScreenController.GameWon -= OnGameWon;
            MapEntryManager.SelectMap -= OnSelectMap;
            VictoryScreen.RestartClick -= OnPlayerRestarted;
            VictoryScreen.SelectMapClick -= OnOpenJournalMenu;
            SettingPanelManager.MainMenuClick -= OnGameQuit;
            SettingPanelManager.GoogleSyncClick -= OnClickSync;
            SettingPanelManager.HowToPlayClick -= OnOpenPopupScreen;
            GoogleSyncScreen.CancelSyncClick -= OnCancelSyncClick;
            GameDataManager.NewGameCompleted -= OnOpenJournalMenu;
            PopupScreen.CloseModalClick -= OnClosePopupScreen;
            GameScreenController.ShowTutorial -= OnOpenPopupScreen;

        }

        void SetVisualElements()
        {
            OutsidePanel = panelScreen.GetOutsidePanel();
            OutsidePanel.style.backgroundColor = new Color(0, 0, 0, 0.3f);
        }

        void RegisterButtonCallbacks()
        {
            //rootElement.AddManipulator(new Clickable(evt => OnOpenGameScreen()));
            OutsidePanel?.RegisterCallback<ClickEvent>(OnOpenGameScreen);
        }

        void SetupModalScreens()
        {
            if (victoryScreen != null)
                allModalScreens.Add(victoryScreen);

            if (panelScreen != null)
                allModalScreens.Add(panelScreen);

            if (loadingScreen != null)
                allModalScreens.Add(loadingScreen);

            if (googleSyncScreen != null)
                allModalScreens.Add(googleSyncScreen);

            if(popupScreen != null)
                allModalScreens.Add(popupScreen);

        }

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

        void OnGameWon(SubmitHistory submit)
        {

            OutsidePanel.SetEnabled(false);
            AudioManager.PlayVictorySound();
            StartCoroutine(GameWonRoutine(submit));
        }

        void OnGameQuit()
        {
            ShowModalScreen(loadingScreen);
            GameQuit?.Invoke();
        }

        IEnumerator GameWonRoutine(SubmitHistory submit)
        {
            IsGameWinning = true;
            victoryScreen.SetSubmitData(submit);
            yield return new WaitForSeconds(1);

            //// hide the UI
            //m_CharPortraitContainer.style.display = DisplayStyle.None;
            //m_PauseButton.style.display = DisplayStyle.None;

            //AudioManager.PlayVictorySound();
            //ShowVisualElement(m_WinScreen, true);
            GamePaused?.Invoke(0f);
            ShowModalScreen(victoryScreen);
            HideGameScreen();
            IsGameWinning = false;
        }

        void ShowGameScreen()
        {
            DefaultInGameScreen.transform.localScale = Vector3.one;
            BlurBackground(false);
            foreach (MenuScreen m in allModalScreens)
            {
                m?.HideScreen();
            }
        }

        void HideGameScreen()
        {
            DefaultInGameScreen.transform.localScale = Vector3.zero;
            BlurBackground(true);
        }

        public void OnOpenJournalMenu()
        {
            if (!IsGameWinning)
            {
                GamePaused?.Invoke(.5f);
                ShowModalScreen(panelScreen);
                panelScreen.ShowLevelPanel(null);
                HideGameScreen();
            }
        }

        public void OnOpenPopupScreen(int contentIndex)
        {
            ShowModalScreen(popupScreen);
            HideGameScreen();
            popupScreen.ShowContent(contentIndex);
        }

        public void OnClosePopupScreen()
        {
            GameResumed?.Invoke();
            ShowGameScreen();
        }

        void OnOpenGameScreen(ClickEvent evt)
        {
            GameResumed?.Invoke();
            ShowGameScreen();
        }

        void OnSelectMap(bool isSameMap)
        {
            AudioManager.PlayLevelStartSound();
            GameRestarted?.Invoke(isSameMap);
            if (isSameMap)
            {
                ShowGameScreen();
            }
            else
            {
                ShowModalScreen(loadingScreen);
            }
        }

        void OnPlayerRestarted()
        {
            OnSelectMap(true);
            OnOpenGameScreen(null);
            OutsidePanel.SetEnabled(true);
        }

        void OnClickSync()
        {
            ShowModalScreen(googleSyncScreen);
        }

        void BlurBackground(bool state)
        {
            if (Volume == null)
                return;

            DepthOfField blurDOF;
            if (Volume.profile.TryGet(out blurDOF))
            {
                blurDOF.active = state;
            }
        }

        public void OnCancelSyncClick(string latestScreen)
        {
            ShowModalScreen(panelScreen);
            panelScreen.ShowSettingPanel(null);
            HideGameScreen();
        }
    }
}