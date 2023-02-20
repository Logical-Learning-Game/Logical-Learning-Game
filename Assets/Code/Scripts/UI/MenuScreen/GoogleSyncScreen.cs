using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System;

namespace Unity.Game.UI
{
    public class GoogleSyncScreen : MenuScreen
    {
        public static event Action CancelSyncClick;
        public static event Action ConfirmSyncClick;
        public static event Action DenySyncClick;

        //public VisualElement LevelPanel;
        //public VisualElement StatPanel;
        //public VisualElement SettingPanel;
        //public VisualElement HistoryPanel;

        Button CancelSyncButton;
        
        VisualElement DetectSaveModal;
        Button ConfirmSyncButton;
        Button DenySyncButton;

        //String ID
        const string CancelSyncButtonName = "CancelSyncButton";
        const string ConfirmSyncButtonName = "ConfirmSyncButton";
        const string DenySyncButtonName = "DenySyncButton";


        void OnEnable()
        {

            //GameScreen.OpenPanel += OnOpenPanel;
        }

        void OnDisable()
        {
            //GameScreen.OpenPanel -= OnOpenPanel;
        }

        protected override void SetVisualElements()
        {
            base.SetVisualElements();

            CancelSyncButton = m_Root.Q<Button>(CancelSyncButtonName);

            DetectSaveModal = m_Root.Q<VisualElement>("DetectSaveModal");
            ConfirmSyncButton = m_Root.Q<Button>(ConfirmSyncButtonName);
            DenySyncButton = m_Root.Q<Button>(DenySyncButtonName);
            
            ShowVisualElement(m_Screen, false);
        }

        protected override void RegisterButtonCallbacks()
        {
            CancelSyncButton?.RegisterCallback<ClickEvent>(ClickCancelSync);
            ConfirmSyncButton?.RegisterCallback<ClickEvent>(ClickConfirmSync);
            DenySyncButton?.RegisterCallback<ClickEvent>(ClickDenySync);
        }

        void ClickCancelSync(ClickEvent evt)
        {
            CancelSyncClick?.Invoke();
        }

        void ClickConfirmSync(ClickEvent evt)
        {
            ConfirmSyncClick?.Invoke();
        }

        void ClickDenySync(ClickEvent evt)
        {
            DenySyncClick?.Invoke();
            Application.Quit();
        }
    }
}