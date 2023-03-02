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
        public static event Action OpenPanel;
        public static event Action<float> MusicVolumeChanged;
        public static event Action<float> SfxVolumeChanged;

        [Header("Blur")]
        [SerializeField] Volume Volume;

        [SerializeField] GameObject DefaultInGameScreen;

        List<MenuScreen> allModalScreens = new List<MenuScreen>();
        [SerializeField] PanelScreen panelScreen;
        [SerializeField] VictoryScreen victoryScreen;
        [SerializeField] LoadingScreen loadingScreen;

        [SerializeField] Button OutsidePanel;


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
            //GameScreenController.SameMapRestart += OnPlayerRestarted;
        }

        void OnDisable()
        {
            GameScreenController.GameWon -= OnGameWon;
            MapEntryManager.SelectMap -= OnSelectMap;
            VictoryScreen.RestartClick -= OnPlayerRestarted;
            VictoryScreen.SelectMapClick -= OnOpenJournalMenu;
            //GameScreenController.SameMapRestart -= OnPlayerRestarted;
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
            OutsidePanel = null;
            AudioManager.PlayVictorySound();
            StartCoroutine(GameWonRoutine(submit));
        }

        IEnumerator GameWonRoutine(SubmitHistory submit)
        {
            victoryScreen.SetSubmitData(submit);
            yield return new WaitForSeconds(1);

            //// hide the UI
            //m_CharPortraitContainer.style.display = DisplayStyle.None;
            //m_PauseButton.style.display = DisplayStyle.None;

            //AudioManager.PlayVictorySound();
            //ShowVisualElement(m_WinScreen, true);

            ShowModalScreen(victoryScreen);
            HideGameScreen();
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
            GamePaused?.Invoke(.5f);
            OpenPanel?.Invoke();

            ShowModalScreen(panelScreen);
            HideGameScreen();
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
    }
}