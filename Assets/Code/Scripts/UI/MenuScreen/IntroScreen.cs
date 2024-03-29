﻿using UnityEngine;
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
            IntroScreenController.DisplayContinueButton += DisplayContinueButton;
 
        }

        void OnDisable()
        {
            IntroScreenController.DisplayContinueButton -= DisplayContinueButton;
        
        }

        protected override void SetVisualElements()
        {
            base.SetVisualElements();

            NewGameButton = m_Root.Q<Button>(NewGameButtonName);
            ContinueButton = m_Root.Q<Button>(ContinueButtonName);
            QuitGameButton = m_Root.Q<Button>(QuitGameButtonName);

            ShowVisualElement(m_Screen, true);
            ShowVisualElement(ContinueButton, false);
        }

        protected override void RegisterButtonCallbacks()
        {
            NewGameButton?.RegisterCallback<ClickEvent>(ClickNewGame);
            ContinueButton?.RegisterCallback<ClickEvent>(ClickContinue);
            QuitGameButton?.RegisterCallback<ClickEvent>(ClickQuitGame);
            NewGameButton?.RegisterCallback<MouseOverEvent>(MouseOverButton);
            ContinueButton?.RegisterCallback<MouseOverEvent>(MouseOverButton);
            QuitGameButton?.RegisterCallback<MouseOverEvent>(MouseOverButton);
        }

        void MouseOverButton(MouseOverEvent evt)
        {
            AudioManager.PlayDefaultHoverSound();
        }

        void ClickNewGame(ClickEvent evt)
        {
            //Debug.Log("ClickNewGame");
            NewGameClick?.Invoke();
            AudioManager.PlayDefaultButtonSound();
        }

        void ClickContinue(ClickEvent evt)
        {
            //Debug.Log("ClickContinue");
            ContinueClick?.Invoke();
            AudioManager.PlayDefaultButtonSound();
        }

        void ClickQuitGame(ClickEvent evt)
        {
            //Debug.Log("ClickQuitGame");
            QuitGameClick?.Invoke();
            AudioManager.PlayDefaultButtonSound();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }

        void DisplayContinueButton()
        {
            ShowVisualElement(ContinueButton, true);
        }
    }
}