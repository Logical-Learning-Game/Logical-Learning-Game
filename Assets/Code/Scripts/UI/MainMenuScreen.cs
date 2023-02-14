using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;
using System.Linq;
using Unity.Game.SaveSystem;

namespace Unity.Game.UI
{
    public class MainMenuScreen : MonoBehaviour
    {
        public static event Action<float> MusicVolumeChanged;
        public static event Action<float> SfxVolumeChanged;

        [Header("Blur")]
        [SerializeField] Volume Volume;
        
        [SerializeField] PanelScreen PanelScreen;
        [SerializeField] VisualElement rootElement;


        void OnEnable()
        {
            SetVisualElements();
            RegisterButtonCallbacks();

            if (Volume == null)
                Volume = FindObjectOfType<Volume>();

        }

        void OnDisable()
        {

        }

        void SetVisualElements()
        {
            PanelScreen = GetComponent<PanelScreen>();
            rootElement = PanelScreen.UIDocument.rootVisualElement;
            //OutsidePanel = PanelScreen.UIDocument.rootVisualElement.Q<Button>("OutsidePanel");
            //ShowVisualElement(rootElement, false);
        }

        void RegisterButtonCallbacks()
        {
            //rootElement.AddManipulator(new Clickable(evt => OnCloseJournalMenu()));
            //OutsidePanel?.RegisterCallback<ClickEvent>(OnCloseJournalMenu);
        }

        void ShowVisualElement(VisualElement visualElement, bool state)
        {
            if (visualElement == null)
                return;

            visualElement.style.display = (state) ? DisplayStyle.Flex : DisplayStyle.None;
        }

    }
}