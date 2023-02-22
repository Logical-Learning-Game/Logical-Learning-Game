using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System;
using Unity.Game.SaveSystem;

namespace Unity.Game.UI
{
    public class GoogleSyncScreenController : MonoBehaviour
    {
        public static event Action ShowDetectSaveModal;
        public static event Action<string,bool> GoogleNewGame;

        [SerializeField] GameData gameData;
        [SerializeField] string currentUserId;

        void Awake()
        {
            //m_ChatData?.AddRange(Resources.LoadAll<ChatSO>(m_ChatResourcePath));
        }

        void OnEnable()
        {
            SaveManager.GameDataLoaded += OnGameDataLoaded;

            NewGameScreenController.GoogleNewGame += OnGoogleSignIn;
            GoogleSyncScreen.DenySyncClick += OnDenySync;
            GoogleSyncScreen.ConfirmSyncClick += OnConfirmSync;
        }

        void OnDisable()
        {
            SaveManager.GameDataLoaded -= OnGameDataLoaded;

            NewGameScreenController.GoogleNewGame -= OnGoogleSignIn;
            GoogleSyncScreen.DenySyncClick -= OnDenySync;
            GoogleSyncScreen.ConfirmSyncClick -= OnConfirmSync;
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

        // scene-management methods
        public void OnGoogleSignIn()
        {
            // TODO for PAT (Google SignIn Method)

            string UserId = "__Google__";

            currentUserId = UserId;
            //when signin completed invoke with its userId
            if(gameData.UserId == "")
            {
                GoogleNewGame?.Invoke(currentUserId,false);
            }
            else
            {
                ShowDetectSaveModal?.Invoke();
            }
            
        }

        public void OnDenySync()
        {
            GoogleNewGame?.Invoke(currentUserId, false);
        }

        public void OnConfirmSync()
        {
            GoogleNewGame?.Invoke(currentUserId, true);
        }
    }
}