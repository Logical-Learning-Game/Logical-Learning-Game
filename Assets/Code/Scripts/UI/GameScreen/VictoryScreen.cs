using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;
using System.Linq;
using Unity.Game.Command;
using Unity.Game.SaveSystem;
using Unity.Game.Level;

namespace Unity.Game.UI
{
    public class VictoryScreen : MenuScreen
    {
        [SerializeField] private Texture2D StarFilled;
        [SerializeField] private Texture2D StarNotFilled;

        public static event Action RestartClick;
        public static event Action SelectMapClick;

        Button RestartButton;
        Button SelectMapButton;

        VisualElement VictoryHeader;
        VisualElement VictoryContainer;

        //String ID
        const string RetryButtonName = "RetryButton";
        const string SelectMapButtonName = "SelectMapButton";

        void OnEnable()
        {

            //GameScreen.OpenPanel += OnOpenPanel;
            //MainMenuUIManager.OpenPanel += OnOpenPanel;
        }

        void OnDisable()
        {
            //GameScreen.OpenPanel -= OnOpenPanel;
            //MainMenuUIManager.OpenPanel -= OnOpenPanel;

        }

        protected override void SetVisualElements()
        {
            base.SetVisualElements();

            VictoryHeader = m_Screen.Q("VictoryHeader");
            VictoryContainer = m_Screen.Q("VictoryContainer");

            RestartButton = m_Screen.Q<Button>("RestartButton");
            SelectMapButton = m_Screen.Q<Button>("ShowNextMapButton");

            ShowVisualElement(m_Screen, false);

        }

        protected override void RegisterButtonCallbacks()
        {
            base.RegisterButtonCallbacks();
            RestartButton?.RegisterCallback<ClickEvent>(OnRestartClick);
            SelectMapButton?.RegisterCallback<ClickEvent>(OnSelectMapClick);
        }

        void OnRestartClick(ClickEvent evt)
        {
            RestartClick?.Invoke();
        }

        void OnSelectMapClick(ClickEvent evt)
        {
            SelectMapClick?.Invoke();
        }

        public void SetSubmitData(SubmitHistory submit)
        {
            //Debug.Log("Setting SubmitData");
            for (int i = 0; i < submit.RuleHistories.Count; i++)
            {
                //Debug.Log($"iterating {i}");
                VictoryHeader.Q($"RuleClearHead{i + 1}").style.backgroundImage = new StyleBackground(submit.RuleHistories[0].IsPass ? StarFilled : StarNotFilled);
                VictoryContainer.Q($"RuleDescriptionContainer{i + 1}").Q("RuleIcon").style.backgroundImage = new StyleBackground(submit.RuleHistories[0].IsPass ? StarFilled : StarNotFilled);
                VictoryContainer.Q($"RuleDescriptionContainer{i + 1}").Q<Label>("RuleDescription").text = LevelManager.gameMap.MapRules[i].GetRule().GetDescription();
            }
        }


    }
}