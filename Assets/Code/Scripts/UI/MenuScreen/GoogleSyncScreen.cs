﻿using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System;

namespace Unity.Game.UI
{
    public class GoogleSyncScreen : MenuScreen
    {
        public static event Action<string> CancelSyncClick;
        public static event Action ConfirmSyncClick;
        public static event Action DenySyncClick;

        Button CancelSyncButton;
        public string LatestScreen;
        
        VisualElement DetectSaveModal;
        Button ConfirmSyncButton;
        Button DenySyncButton;

        //String ID
        const string CancelSyncButtonName = "CancelSyncButton";
        const string ConfirmSyncButtonName = "ConfirmSyncButton";
        const string DenySyncButtonName = "DenySyncButton";


        void OnEnable()
        {

            GoogleSyncScreenController.ShowDetectSaveModal += OnLocalSaveExisted;
        }

        void OnDisable()
        {
            GoogleSyncScreenController.ShowDetectSaveModal -= OnLocalSaveExisted;
        }

        protected override void SetVisualElements()
        {
            base.SetVisualElements();

            CancelSyncButton = m_Screen.Q<Button>(CancelSyncButtonName);

            DetectSaveModal = m_Screen.Q<VisualElement>("DetectSaveModal");
            ConfirmSyncButton = m_Screen.Q<Button>(ConfirmSyncButtonName);
            DenySyncButton = m_Screen.Q<Button>(DenySyncButtonName);


            
            ShowVisualElement(m_Screen, false);
            ShowVisualElement(DetectSaveModal, false);
        }

        protected override void RegisterButtonCallbacks()
        {
            CancelSyncButton?.RegisterCallback<ClickEvent>(ClickCancelSync);
            ConfirmSyncButton?.RegisterCallback<ClickEvent>(ClickConfirmSync);
            DenySyncButton?.RegisterCallback<ClickEvent>(ClickDenySync);

            CancelSyncButton?.RegisterCallback<MouseOverEvent>(MouseOverButton);
            ConfirmSyncButton?.RegisterCallback<MouseOverEvent>(MouseOverButton);
            DenySyncButton?.RegisterCallback<MouseOverEvent>(MouseOverButton);
        }


        void MouseOverButton(MouseOverEvent evt)
        {
            AudioManager.PlayDefaultHoverSound();
        }
        void ClickCancelSync(ClickEvent evt)
        {
            CancelSyncClick?.Invoke(LatestScreen);
            AudioManager.PlayDefaultWarningSound();
        }

        void ClickConfirmSync(ClickEvent evt)
        {
            ConfirmSyncClick?.Invoke();
            AudioManager.PlayDefaultButtonSound();
        }

        void ClickDenySync(ClickEvent evt)
        {
            DenySyncClick?.Invoke();
            AudioManager.PlayDefaultButtonSound();
        }

        void OnLocalSaveExisted()
        {
            ShowVisualElement(DetectSaveModal, true);
        }
    }
}