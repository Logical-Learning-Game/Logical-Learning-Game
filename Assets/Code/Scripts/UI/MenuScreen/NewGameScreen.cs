﻿using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System;


namespace Unity.Game.UI
{
    public class NewGameScreen : MenuScreen
    {
        public static event Action LocalNewGameClick;
        public static event Action GoogleNewGameClick;
        public static event Action BackClick;
        public static event Action LocalNewGameConfirm;

        Button LocalNewGameButton;
        Button GoogleNewGameButton;
        Button BackButton;

        VisualElement DetectSaveModal;
        Button CancelNewGameButton;
        Button ConfirmNewGameButton;

        //String ID
        const string LocalNewGameButtonName = "LocalNewGameButton";
        const string GoogleNewGameButtonName = "GoogleNewGameButton";
        const string BackButtonName = "BackButton";


        void OnEnable()
        {
            NewGameScreenController.ShowDetectSaveModal += OnLocalSaveExisted;
        }

        void OnDisable()
        {
            NewGameScreenController.ShowDetectSaveModal -= OnLocalSaveExisted;

        }

        protected override void SetVisualElements()
        {
            base.SetVisualElements();
            
            LocalNewGameButton = m_Screen.Q<Button>(LocalNewGameButtonName);
            GoogleNewGameButton = m_Screen.Q<Button>(GoogleNewGameButtonName);
            BackButton = m_Screen.Q<Button>(BackButtonName);

            DetectSaveModal = m_Screen.Q<VisualElement>("DetectSaveModal");
            CancelNewGameButton = m_Screen.Q<Button>("CancelNewGameButton");
            ConfirmNewGameButton = m_Screen.Q<Button>("ConfirmNewGameButton");
            
            ShowVisualElement(m_Screen, false);
            ShowVisualElement(DetectSaveModal, false);
        }

        protected override void RegisterButtonCallbacks()
        {
            LocalNewGameButton?.RegisterCallback<ClickEvent>(ClickLocalNewGame);
            GoogleNewGameButton?.RegisterCallback<ClickEvent>(ClickGoogleNewGame);
            BackButton?.RegisterCallback<ClickEvent>(ClickBack);

            CancelNewGameButton?.RegisterCallback<ClickEvent>(ClickCancelNewGame);
            ConfirmNewGameButton?.RegisterCallback<ClickEvent>(ClickConfirmNewGame);

            LocalNewGameButton?.RegisterCallback<MouseOverEvent>(MouseOverButton);
            GoogleNewGameButton?.RegisterCallback<MouseOverEvent>(MouseOverButton);
            BackButton?.RegisterCallback<MouseOverEvent>(MouseOverButton);

            CancelNewGameButton?.RegisterCallback<MouseOverEvent>(MouseOverButton);
            ConfirmNewGameButton?.RegisterCallback<MouseOverEvent>(MouseOverButton);
        }

        void MouseOverButton(MouseOverEvent evt)
        {
            AudioManager.PlayDefaultHoverSound();
        }

        void ClickLocalNewGame(ClickEvent evt)
        {
            LocalNewGameClick?.Invoke();
            AudioManager.PlayDefaultButtonSound();
        }

        void ClickGoogleNewGame(ClickEvent evt)
        {
            GoogleNewGameClick?.Invoke();
            AudioManager.PlayDefaultButtonSound();
        }

        void ClickBack(ClickEvent evt)
        {
            BackClick?.Invoke();
            AudioManager.PlayDefaultWarningSound();
        }

        void ClickCancelNewGame(ClickEvent evt)
        {
            ShowVisualElement(DetectSaveModal, false);
            AudioManager.PlayDefaultWarningSound();

        }

        void ClickConfirmNewGame(ClickEvent evt)
        {
            LocalNewGameConfirm?.Invoke();
            ShowVisualElement(DetectSaveModal, false);
            AudioManager.PlayDefaultButtonSound();
        }

        void OnLocalSaveExisted()
        {
            ShowVisualElement(DetectSaveModal, true);
        }
    }
}