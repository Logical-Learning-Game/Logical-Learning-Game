using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;
using System.Linq;

namespace Unity.Game.UI
{
    public class GameScreen : MonoBehaviour
    {

        public static event Action<float> GamePaused;
        public static event Action GameResumed;
        public static event Action GameQuit;
        public static event Action GameRestarted;
        public static event Action<float> MusicVolumeChanged;
        public static event Action<float> SfxVolumeChanged;

        [Header("Menu Screen elements")]
        [Tooltip("String IDs to query Visual Elements")]
        [SerializeField] string PauseScreenName = "PauseScreen";
        [SerializeField] string WinScreenName = "GameWinScreen";

        [Header("Blur")]
        [SerializeField] Volume Volume;
        
        [SerializeField] GameObject DefaultInGameScreen;
        [SerializeField] PanelScreen PanelScreen;
        [SerializeField] VisualElement rootElement;
        [SerializeField] Button OutsidePanel;


        void OnEnable()
        {
            SetVisualElements();
            RegisterButtonCallbacks();

            if (Volume == null)
                Volume = FindObjectOfType<Volume>();

            GameScreenController.GameWon += OnGameWon;
        }

        void OnDisable()
        {
            GameScreenController.GameWon -= OnGameWon;
        }

        void SetVisualElements()
        {
            PanelScreen = GetComponent<PanelScreen>();
            rootElement = PanelScreen.GameScreen.rootVisualElement;
            OutsidePanel = PanelScreen.GameScreen.rootVisualElement.Q<Button>("OutsidePanel");
            ShowVisualElement(rootElement, false);
        }

        void RegisterButtonCallbacks()
        {
            //rootElement.AddManipulator(new Clickable(evt => OnCloseJournalMenu()));
            OutsidePanel?.RegisterCallback<ClickEvent>(OnCloseJournalMenu);
        }

        void ShowVisualElement(VisualElement visualElement, bool state)
        {
            if (visualElement == null)
                return;

            visualElement.style.display = (state) ? DisplayStyle.Flex : DisplayStyle.None;
        }

        void OnGameWon()
        {
            StartCoroutine(GameWonRoutine());
        }

        IEnumerator GameWonRoutine()
        {
            yield return new WaitForSeconds(1);

            //// hide the UI
            //m_CharPortraitContainer.style.display = DisplayStyle.None;
            //m_PauseButton.style.display = DisplayStyle.None;

            //AudioManager.PlayVictorySound();
            //ShowVisualElement(m_WinScreen, true);
        }

        public void OnOpenJournalMenu()
        {
            GamePaused?.Invoke(.5f);
            BlurBackground(true);
            ShowVisualElement(rootElement, true);
            DefaultInGameScreen.transform.localScale = Vector3.zero;
        }

        void OnCloseJournalMenu(ClickEvent evt)
        {
            GameResumed?.Invoke();
            BlurBackground(false);
            ShowVisualElement(rootElement, false);
            DefaultInGameScreen.transform.localScale = Vector3.one;
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