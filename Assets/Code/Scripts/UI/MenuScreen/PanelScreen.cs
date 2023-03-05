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
            //UserStatManager.UpdateUserStat += DisplayUserStat;
        }

        void OnDisable()
        {
            GameScreen.OpenPanel -= OnOpenPanel;
            MainMenuUIManager.OpenPanel -= OnOpenPanel;
            //UserStatManager.UpdateUserStat -= DisplayUserStat;
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
            AudioManager.PlayDefaultButtonSound();

        }
        void ShowStatPanel(ClickEvent evt)
        {
            SwitchPanel(StatPanel);
            OpenStatPanel?.Invoke();
            AudioManager.PlayDefaultButtonSound();
        }
        void ShowSettingPanel(ClickEvent evt)
        {
            SwitchPanel(SettingPanel);
            OpenSettingPanel?.Invoke();
            AudioManager.PlayDefaultButtonSound();
        }
        void ShowHistoryPanel(ClickEvent evt)
        {
            SwitchPanel(HistoryPanel);
            OpenHistoryPanel?.Invoke();
            AudioManager.PlayDefaultButtonSound();
        }

        void OnOpenPanel()
        {
            ShowLevelPanel(null);
        }

        //public void DisplayUserStat(List<int> displayValue)
        //{
        //    Debug.Log(string.Join(",", displayValue));
        //    displayValue Order based on UserStatManager Declaration
        //    StatPanel.Q("StarSummary").Q<Label>("StarSumValue").text = (displayValue[3] + displayValue[4] + displayValue[5]).ToString();
        //    StatPanel.Q("StarSummary").Q<Label>("StarSumMax").text = (displayValue[0] + displayValue[1] + displayValue[2]).ToString();

        //    StatPanel.Q("NormalStarSummary").Q<Label>("StarSumMax").text = (displayValue[0]).ToString();
        //    StatPanel.Q("NormalStarSummary").Q<Label>("StarSumValue").text = (displayValue[3]).ToString();

        //    StatPanel.Q("ConditionStarSummary").Q<Label>("StarSumMax").text = (displayValue[1]).ToString();
        //    StatPanel.Q("ConditionStarSummary").Q<Label>("StarSumValue").text = (displayValue[4]).ToString();

        //    StatPanel.Q("LoopStarSummary").Q<Label>("StarSumMax").text = (displayValue[2]).ToString();
        //    StatPanel.Q("LoopStarSummary").Q<Label>("StarSumValue").text = (displayValue[5]).ToString();

        //    StatPanel.Q("MedalSummary").Q<Label>("StarSumValue").text = (displayValue[6] + displayValue[7] + displayValue[8]).ToString();

        //    StatPanel.Q("GoldMedalSummary").Q<Label>("StarSumValue").text = (displayValue[6]).ToString();
        //    StatPanel.Q("SilverMedalSummary").Q<Label>("StarSumValue").text = (displayValue[7]).ToString();
        //    StatPanel.Q("BronzeMedalSummary").Q<Label>("StarSumValue").text = (displayValue[8]).ToString();

        //}


    }
}