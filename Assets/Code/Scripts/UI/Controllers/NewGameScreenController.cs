using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System;
using Unity.Game.SaveSystem;

namespace Unity.Game.UI
{
    public class NewGameScreenController : MonoBehaviour
    {
        public static event Action ShowDetectSaveModal;
        public static event Action<string> LocalNewGame;
        public static event Action GoogleNewGame;

        GameData gameData;

        void Awake()
        {
            //m_ChatData?.AddRange(Resources.LoadAll<ChatSO>(m_ChatResourcePath));
        }

        void OnEnable()
        {
            NewGameScreen.LocalNewGameClick += OnLocalNewGameClick;
            NewGameScreen.GoogleNewGameClick += OnGoogleNewGameClick;
            NewGameScreen.LocalNewGameConfirm += NewGameAsGuest;

            SaveManager.GameDataLoaded += OnGameDataLoaded;
        }

        void OnDisable()
        {
            NewGameScreen.LocalNewGameClick -= OnLocalNewGameClick;
            NewGameScreen.GoogleNewGameClick -= OnGoogleNewGameClick;
            NewGameScreen.LocalNewGameConfirm -= NewGameAsGuest;

            SaveManager.GameDataLoaded -= OnGameDataLoaded;
        }

        void Start()
        {
            //ShowLevelInfo?.Invoke(m_LevelData);
            //ShowChats?.Invoke(m_ChatData);
        }

        void OnGameDataLoaded(GameData gameData)
        {
            this.gameData = gameData;
        }

        public void OnLocalNewGameClick()
        {

            if (gameData == null || gameData?.PlayerId == "")
            {
                NewGameAsGuest();
            }
            else
            {
                ShowDetectSaveModal?.Invoke();
            }

        }

        public void OnGoogleNewGameClick()
        {
            GoogleNewGame?.Invoke();
        }

        public void NewGameAsGuest()
        {
            LocalNewGame?.Invoke("__guest__");
        }

    }
}