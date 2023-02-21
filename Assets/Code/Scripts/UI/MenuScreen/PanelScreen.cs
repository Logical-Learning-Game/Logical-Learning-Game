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
    public class PanelScreen : MenuScreen
    {
        public static event Action OpenLevelPanel;
        public static event Action OpenStatPanel;
        public static event Action OpenSettingPanel;
        public static event Action OpenHistoryPanel;

        public VisualElement LevelPanel;
        public VisualElement StatPanel;
        public VisualElement SettingPanel;
        public VisualElement HistoryPanel;

        Button LevelButton;
        Button StatButton;
        Button SettingButton;
        Button HistoryButton;

        //String ID
        const string LevelButtonName = "LevelButton";
        const string StatButtonName = "StatButton";
        const string SettingButtonName = "SettingButton";
        const string HistoryButtonName = "HistoryButton";

        void OnEnable()
        {

            GameScreen.OpenPanel += OnOpenPanel;
            MainMenuUIManager.OpenPanel += OnOpenPanel;
        }

        void OnDisable()
        {
            GameScreen.OpenPanel -= OnOpenPanel;
            MainMenuUIManager.OpenPanel -= OnOpenPanel;

        }

        protected override void SetVisualElements()
        {
            base.SetVisualElements();

            LevelPanel = m_Root.Q("LevelPanel");
            StatPanel = m_Root.Q("StatsPanel");
            SettingPanel = m_Root.Q("SettingPanel");
            HistoryPanel = m_Root.Q("HistoryPanel");

            LevelButton = m_Root.Q<Button>(LevelButtonName);
            StatButton = m_Root.Q<Button>(StatButtonName);
            SettingButton = m_Root.Q<Button>(SettingButtonName);
            HistoryButton = m_Root.Q<Button>(HistoryButtonName);

            ShowVisualElement(m_Screen, false);

        }

        protected override void RegisterButtonCallbacks()
        {
            base.RegisterButtonCallbacks();
            LevelButton?.RegisterCallback<ClickEvent>(ShowLevelPanel);
            StatButton?.RegisterCallback<ClickEvent>(ShowStatPanel);
            SettingButton?.RegisterCallback<ClickEvent>(ShowSettingPanel);
            HistoryButton?.RegisterCallback<ClickEvent>(ShowHistoryPanel);
        }

        public Button GetOutsidePanel()
        {
            return m_Root.Q<Button>("OutsidePanel"); 
        }

        void SwitchPanel(VisualElement visualElement)
        {
            //Debug.Log("switchpanel");
            if (visualElement == null)
                return;

            ShowVisualElement(LevelPanel, false);
            ShowVisualElement(StatPanel, false);
            ShowVisualElement(SettingPanel, false);
            ShowVisualElement(HistoryPanel, false);

            ShowVisualElement(visualElement, true);
        }

        void ShowLevelPanel(ClickEvent evt)
        {
            SwitchPanel(LevelPanel);
            OpenLevelPanel?.Invoke();
        }
        void ShowStatPanel(ClickEvent evt)
        {
            SwitchPanel(StatPanel);
            OpenStatPanel?.Invoke();
        }
        void ShowSettingPanel(ClickEvent evt)
        {
            SwitchPanel(SettingPanel);
            OpenSettingPanel?.Invoke();
        }
        void ShowHistoryPanel(ClickEvent evt)
        {
            SwitchPanel(HistoryPanel);
            OpenHistoryPanel?.Invoke();
        }

        void OnOpenPanel()
        {
            ShowLevelPanel(new ClickEvent());
        }
    }
}