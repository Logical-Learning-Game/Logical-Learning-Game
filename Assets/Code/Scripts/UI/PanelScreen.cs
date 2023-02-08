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
    public class PanelScreen : MonoBehaviour
    {
        public UIDocument GameScreen;
        VisualElement LevelPanel;
        VisualElement StatPanel;
        VisualElement SettingPanel;
        VisualElement HistoryPanel;

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
            SetVisualElements();
            RegisterButtonCallbacks();
        }

        void OnDisable()
        {

        }

        void SetVisualElements()
        {
            GameScreen = GetComponent<UIDocument>();
            VisualElement rootElement = GameScreen.rootVisualElement;

            LevelPanel = GameScreen.rootVisualElement.Q("LevelPanel");
            StatPanel = GameScreen.rootVisualElement.Q("StatPanel");
            SettingPanel = GameScreen.rootVisualElement.Q("SettingPanel");
            HistoryPanel = GameScreen.rootVisualElement.Q("HistoryPanel");

            LevelButton = rootElement.Q<Button>(LevelButtonName);
            StatButton = rootElement.Q<Button>(StatButtonName);
            SettingButton = rootElement.Q<Button>(SettingButtonName);
            HistoryButton = rootElement.Q<Button>(HistoryButtonName);

        }

        void RegisterButtonCallbacks()
        {
            LevelButton?.RegisterCallback<ClickEvent>(ShowLevelPanel);
            StatButton?.RegisterCallback<ClickEvent>(ShowStatPanel);
            SettingButton?.RegisterCallback<ClickEvent>(ShowSettingPanel);
            HistoryButton?.RegisterCallback<ClickEvent>(ShowHistoryPanel);
        }

        void ShowVisualElement(VisualElement visualElement, bool state)
        {
            if (visualElement == null)
                return;

            visualElement.style.display = (state) ? DisplayStyle.Flex : DisplayStyle.None;
        }

        void SwitchPanel(VisualElement visualElement)
        {
            if (visualElement == null)
                return;

            Debug.Log("SwitchPanel");
            ShowVisualElement(LevelPanel, false);
            ShowVisualElement(StatPanel, false);
            ShowVisualElement(SettingPanel, false);
            ShowVisualElement(HistoryPanel, false);

            ShowVisualElement(visualElement, true);
        }

        void ShowLevelPanel(ClickEvent evt)
        {
            SwitchPanel(LevelPanel);
        }
        void ShowStatPanel(ClickEvent evt)
        {
            SwitchPanel(StatPanel);
        }
        void ShowSettingPanel(ClickEvent evt)
        {
            SwitchPanel(SettingPanel);
        }
        void ShowHistoryPanel(ClickEvent evt)
        {
            SwitchPanel(HistoryPanel);
        }

    }
}