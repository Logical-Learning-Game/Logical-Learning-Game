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
        [SerializeField] PanelScreen PanelScreen;
        [SerializeField] VictoryScreen VictoryScreen;
        
        [SerializeField] Button OutsidePanel;


        void OnEnable()
        {
            SetVisualElements();
            RegisterButtonCallbacks();

            if (Volume == null)
                Volume = FindObjectOfType<Volume>();

            GameScreenController.GameWon += OnGameWon;
            MapEntryManager.SelectMap += OnSelectMap;
            VictoryScreen.RestartClick += OnPlayerRestarted;
            //GameScreenController.SameMapRestart += OnPlayerRestarted;
        }

        void OnDisable()
        {
            GameScreenController.GameWon -= OnGameWon;
            MapEntryManager.SelectMap -= OnSelectMap;
            VictoryScreen.RestartClick -= OnPlayerRestarted;
            //GameScreenController.SameMapRestart -= OnPlayerRestarted;
        }

        void SetVisualElements()
        {
            OutsidePanel = PanelScreen.GetOutsidePanel();
            OutsidePanel.style.backgroundColor = new Color(0, 0, 0, 0.3f);
        }

        void RegisterButtonCallbacks()
        {
            //rootElement.AddManipulator(new Clickable(evt => OnOpenGameScreen()));
            OutsidePanel?.RegisterCallback<ClickEvent>(OnOpenGameScreen);
        }

        void OnGameWon(SubmitHistory submit)
        {
            OutsidePanel = null;
            StartCoroutine(GameWonRoutine(submit));
        }

        IEnumerator GameWonRoutine(SubmitHistory submit)
        {
            yield return new WaitForSeconds(1);

            //// hide the UI
            //m_CharPortraitContainer.style.display = DisplayStyle.None;
            //m_PauseButton.style.display = DisplayStyle.None;

            //AudioManager.PlayVictorySound();
            //ShowVisualElement(m_WinScreen, true);
            VictoryScreen.SetSubmitData(submit);
            VictoryScreen.ShowScreen();
            BlurBackground(true);
            DefaultInGameScreen.transform.localScale = Vector3.zero;
        }

        public void OnOpenJournalMenu()
        {
            GamePaused?.Invoke(.5f);
            OpenPanel?.Invoke();
            BlurBackground(true);
            PanelScreen.ShowScreen();
            VictoryScreen.HideScreen();
            DefaultInGameScreen.transform.localScale = Vector3.zero;
        }

        void OnOpenGameScreen(ClickEvent evt)
        {
            GameResumed?.Invoke();
            BlurBackground(false);
            PanelScreen.HideScreen();
            VictoryScreen.HideScreen();
            DefaultInGameScreen.transform.localScale = Vector3.one;
        }

        void OnSelectMap(bool isSameMap)
        {
            GameRestarted?.Invoke(isSameMap);
            OnOpenGameScreen(null);
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