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
        public static event Action OpenLevelPanel;
        public static event Action OpenStatPanel;
        public static event Action OpenSettingPanel;
        public static event Action OpenHistoryPanel;

        public UIDocument UIDocument;
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
            SetVisualElements();
            RegisterButtonCallbacks();

            GameScreen.OpenPanel += OnOpenPanel;
        }

        void OnDisable()
        {
            GameScreen.OpenPanel -= OnOpenPanel;
        }

        void SetVisualElements()
        {
            UIDocument = GetComponent<UIDocument>();
            VisualElement rootElement = UIDocument.rootVisualElement;

            LevelPanel = UIDocument.rootVisualElement.Q("LevelPanel");
            StatPanel = UIDocument.rootVisualElement.Q("StatPanel");
            SettingPanel = UIDocument.rootVisualElement.Q("SettingPanel");
            HistoryPanel = UIDocument.rootVisualElement.Q("HistoryPanel");

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

            //Debug.Log("SwitchPanel");
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