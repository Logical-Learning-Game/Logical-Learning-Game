using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System;

namespace Unity.Game.UI
{
    public class NewGameScreen : MenuScreen
    {
        public static event Action LocalNewGameClick;
        public static event Action GoogleNewGameClick;
        public static event Action BackClick;
        

        Button LocalNewGameButton;
        Button GoogleNewGameButton;
        Button BackButton;

        VisualElement DetectSaveModal;
        Button CancelNewGameButton;
        Button ConfirmNewGameButton;

        //String ID
        const string LocalNewGameButtonName = "LocalNewGameButton";
        const string GoogleNewGameButtonName = "GoogleNewGameButton";
        const string BackButtonName = "BackButton";


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
            
            LocalNewGameButton = m_Root.Q<Button>(LocalNewGameButtonName);
            GoogleNewGameButton = m_Root.Q<Button>(GoogleNewGameButtonName);
            BackButton = m_Root.Q<Button>(BackButtonName);

            DetectSaveModal = m_Screen.Q<VisualElement>("DetectSaveModal");
            CancelNewGameButton = m_Screen.Q<Button>("CancelNewGameButton");
            ConfirmNewGameButton = m_Screen.Q<Button>("ConfirmNewGameButton");
            
            ShowVisualElement(m_Screen, false);
        }

        protected override void RegisterButtonCallbacks()
        {
            LocalNewGameButton?.RegisterCallback<ClickEvent>(ClickLocalNewGame);
            GoogleNewGameButton?.RegisterCallback<ClickEvent>(ClickGoogleNewGame);
            BackButton?.RegisterCallback<ClickEvent>(ClickBack);

            CancelNewGameButton?.RegisterCallback<ClickEvent>(ClickCancelNewGame);
            ConfirmNewGameButton?.RegisterCallback<ClickEvent>(ClickConfirmNewGame);
        }

        void ClickLocalNewGame(ClickEvent evt)
        {
            // if player have local save, show modal
            // else, go to local new game
            Debug.Log("click local newgame");
            Debug.Log(DetectSaveModal.name);
            if (true)
            {
                ShowVisualElement(DetectSaveModal,true);
            }
            else
            {
                LocalNewGameClick?.Invoke();
            }
            
        }

        void ClickGoogleNewGame(ClickEvent evt)
        {
            GoogleNewGameClick?.Invoke();
        }

        void ClickBack(ClickEvent evt)
        {
            BackClick?.Invoke();
           
        }

        void ClickCancelNewGame(ClickEvent evt)
        {
            ShowVisualElement(DetectSaveModal, false);
        }

        void ClickConfirmNewGame(ClickEvent evt)
        {
            ShowVisualElement(DetectSaveModal, false);
            LocalNewGameClick?.Invoke();
        }
    }
}