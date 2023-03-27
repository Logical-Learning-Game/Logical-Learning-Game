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

        public static event Action CloseModalClick;

        Button ContinueButton;
        Button PreviousButton;
        Button NextButton;

        VisualElement PopupImage;

        List<Texture2D> ImageList;
        int currentIndex;

        [SerializeField] List<Texture2D> FirstPlayTutorial;
        [SerializeField] List<Texture2D> LoopTutorial;
        [SerializeField] List<Texture2D> ConditionTutorial;
        [SerializeField] List<Texture2D> ItemTutorial;

        //String ID
        const string PopupImageName = "PopupImage";
        const string ContinueButtonName = "ContinueButton";
        const string PreviousButtonName = "PreviousButton";
        const string NextButtonName = "NextButton";

        void OnEnable()
        {

 
        }

        void OnDisable()
        {


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
            
            // if in last page of image, invoke close modal
            if (currentIndex == ImageList.Count - 1)
            {
                CloseModalClick?.Invoke();
            }
            else
            {
                AudioManager.PlayDefaultButtonSound();
                UpdateImageIndex(1);
            }

        }

        void OnPreviousClick(ClickEvent evt)
        {
            AudioManager.PlayDefaultButtonSound();
            UpdateImageIndex(-1);
        }

        void OnNextClick(ClickEvent evt)
        {
            AudioManager.PlayDefaultButtonSound();
            UpdateImageIndex(1);
        }

        public void SetModalData(List<Texture2D> ImageList)
        {
            this.ImageList = ImageList;
            currentIndex = 0;
            UpdateImageIndex();
        }

        public void UpdateImageIndex(int value = 0)
        {
            currentIndex += value;
            //check if there is data
            if (ImageList.Count > 0)
            {
                PopupImage.style.backgroundImage = ImageList[currentIndex];
                if (ImageList.Count > 1)
                {
                    PreviousButton.style.display = DisplayStyle.Flex;
                    NextButton.style.display = DisplayStyle.Flex;
                    if (currentIndex == 0)
                    {
                        PreviousButton.SetEnabled(false);
                    }
                    else
                    {
                        PreviousButton.SetEnabled(true);
                    }

                    if (currentIndex == ImageList.Count - 1)
                    {
                        NextButton.SetEnabled(false);
                    }
                    else
                    {
                        NextButton.SetEnabled(true);
                    }
                }
                else
                {
                    PreviousButton.style.display = DisplayStyle.None;
                    NextButton.style.display = DisplayStyle.None;
                }

                if (currentIndex == ImageList.Count - 1)
                {
                    ContinueButton.text = "Continue";
                }
                else
                {
                    ContinueButton.text = "Next";
                }
            }
            else
            {
                PreviousButton.style.display = DisplayStyle.None;
                NextButton.style.display = DisplayStyle.None;
            }
            

        }

        public void ShowContent(int contentType = -1)
        {
            SetModalData(GetPopupContent(contentType));
        }

        public List<Texture2D> GetPopupContent(int contentType = -1)
        {
            //contentType is type of content that will be serve,
            //-1 is default (serve all reached)
            // 0 is tutorial only
            // 1 is loop only
            // 2 is condition only
            // 3 is item only
            List<Texture2D> popupContent = new List<Texture2D>();
            switch (contentType)
            {
                case -1:
                    popupContent = FirstPlayTutorial.Concat(LoopTutorial).Concat(ConditionTutorial).Concat(ItemTutorial).ToList();
                    break;
                case 0:
                    popupContent = FirstPlayTutorial;
                    break;
                case 1:
                    popupContent = LoopTutorial;
                    break;
                case 2:
                    popupContent = ConditionTutorial;
                    break;
                case 3:
                    popupContent = ItemTutorial;
                    break;
                default: break;
            }

            return popupContent;
        }

    }
}