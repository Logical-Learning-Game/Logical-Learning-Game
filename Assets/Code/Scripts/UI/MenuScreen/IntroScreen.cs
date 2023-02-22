using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System;

namespace Unity.Game.UI
{
    public class IntroScreen : MenuScreen
    {
        public static event Action NewGameClick;
        public static event Action ContinueClick;
        public static event Action QuitGameClick;

        //public VisualElement LevelPanel;
        //public VisualElement StatPanel;
        //public VisualElement SettingPanel;
        //public VisualElement HistoryPanel;

        Button NewGameButton;
        Button ContinueButton;
        Button QuitGameButton;

        //String ID
        const string NewGameButtonName = "NewGameButton";
        const string ContinueButtonName = "ContinueButton";
        const string QuitGameButtonName = "QuitGameButton";

        void OnEnable()
        {
            IntroScreenController.HideContinueButton += HideContinueButton;
            //GameScreen.OpenPanel += OnOpenPanel;
        }

        void OnDisable()
        {
            IntroScreenController.HideContinueButton -= HideContinueButton;
            //GameScreen.OpenPanel -= OnOpenPanel;
        }

        protected override void SetVisualElements()
        {
            base.SetVisualElements();

            NewGameButton = m_Root.Q<Button>(NewGameButtonName);
            ContinueButton = m_Root.Q<Button>(ContinueButtonName);
            QuitGameButton = m_Root.Q<Button>(QuitGameButtonName);

            ShowVisualElement(m_Screen, true);
        }

        protected override void RegisterButtonCallbacks()
        {
            NewGameButton?.RegisterCallback<ClickEvent>(ClickNewGame);
            ContinueButton?.RegisterCallback<ClickEvent>(ClickContinue);
            QuitGameButton?.RegisterCallback<ClickEvent>(ClickQuitGame);
        }

        void ClickNewGame(ClickEvent evt)
        {
            Debug.Log("ClickNewGame");
            NewGameClick?.Invoke();
        }

        void ClickContinue(ClickEvent evt)
        {
            Debug.Log("ClickContinue");
            ContinueClick?.Invoke();
        }

        void ClickQuitGame(ClickEvent evt)
        {
            Debug.Log("ClickQuitGame");
            QuitGameClick?.Invoke();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }

        void HideContinueButton()
        {
            ShowVisualElement(ContinueButton, false);
        }
    }
}