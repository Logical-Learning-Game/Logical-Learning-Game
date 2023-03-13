using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using GlobalConfig;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;
using System.Linq;
using Unity.Game.Command;
using Unity.Game.SaveSystem;
using Unity.Game.Level;

namespace Unity.Game.UI
{
    public class PopupScreen : MenuScreen
    {
        //[SerializeField] private Texture2D StarFilled;
        //[SerializeField] private Texture2D StarNotFilled;

        public static event Action CloseModalClick;

        Button ContinueButton;
        Button PreviousButton;
        Button NextButton;

        VisualElement PopupImage;

        List<Texture2D> ImageList;
        //VisualElement VictoryHeader;
        //VisualElement VictoryContainer;

        //String ID
        const string PopupImageName = "PopupImage";
        const string ContinueButtonName = "ContinueButton";
        const string PreviousButtonName = "PreviousButton";
        const string NextButtonName = "NextButton";

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

            PopupImage = m_Screen.Q(PopupImageName);

            ContinueButton = m_Screen.Q<Button>(ContinueButtonName);
            PreviousButton = m_Screen.Q<Button>(PreviousButtonName);
            NextButton = m_Screen.Q<Button>(NextButtonName);

            ShowVisualElement(m_Screen, false);

        }

        protected override void RegisterButtonCallbacks()
        {
            base.RegisterButtonCallbacks();
            ContinueButton?.RegisterCallback<ClickEvent>(OnContinueClick);
            PreviousButton?.RegisterCallback<ClickEvent>(OnPreviousClick);
            NextButton?.RegisterCallback<ClickEvent>(OnNextClick);
            ContinueButton?.RegisterCallback<MouseOverEvent>(MouseOverButton);
            PreviousButton?.RegisterCallback<MouseOverEvent>(MouseOverButton);
            NextButton?.RegisterCallback<MouseOverEvent>(MouseOverButton);

        }

        void MouseOverButton(MouseOverEvent evt)
        {
            AudioManager.PlayDefaultHoverSound();
        }

        void OnContinueClick(ClickEvent evt)
        {
            AudioManager.PlayDefaultButtonSound();
            // if in last page of image, invoke close modal
            CloseModalClick?.Invoke();
        }

        void OnPreviousClick(ClickEvent evt)
        {
            AudioManager.PlayDefaultButtonSound();
        }

        void OnNextClick(ClickEvent evt)
        {
            AudioManager.PlayDefaultButtonSound();
        }

        public void SetModalData()
        {
            
        }


    }
}