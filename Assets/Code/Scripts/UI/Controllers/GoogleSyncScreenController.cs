using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using Unity.Game.SaveSystem;
using System.Net;
using Unity.Game.Authentication;
using Unity.Services.Core;
using Unity.Services.Authentication;

namespace Unity.Game.UI
{
    public class GoogleSyncScreenController : MonoBehaviour
    {
        public static event Action ShowDetectSaveModal;
        public static event Action<string,bool> GoogleNewGame;

        [SerializeField] GameData gameData;
        [SerializeField] string currentPlayerId;

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

            SettingPanelManager.GoogleSyncClick += OnGoogleSignIn;
        }

        void OnDisable()
        {
            SaveManager.GameDataLoaded -= OnGameDataLoaded;

            NewGameScreenController.GoogleNewGame -= OnGoogleSignIn;
            GoogleSyncScreen.DenySyncClick -= OnDenySync;
            GoogleSyncScreen.ConfirmSyncClick -= OnConfirmSync;

            SettingPanelManager.GoogleSyncClick -= OnGoogleSignIn;
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
        public async void OnGoogleSignIn()
        {
            // google signin
            try
            {
                if(GoogleAuthenticationManager.Instance == null)
                {
                    gameObject.AddComponent<GoogleAuthenticationManager>();
                }
                string playerId = await GoogleAuthenticationManager.Instance.GoogleSignIn();
                Debug.Log($"PlayerId: {playerId}");
                currentPlayerId = playerId;

                var apiClient = new APIClient();

                // link account at server
                bool accountLinked = await apiClient.AccountCheck(playerId);
                if (!accountLinked)
                {
                    var linkAccountRequest = new LinkAccountRequest
                    {
                        PlayerId = playerId,
                        Email = "basleng@hotmail.com"
                    };
                    await apiClient.LinkAccount(linkAccountRequest);
                }

                OnSignInComplete(currentPlayerId);
            }
            catch (WebException ex)
            {
                Debug.LogException(ex);
            }
            catch (AuthenticationException ex)
            {
                Debug.LogException(ex);
            }
            catch (RequestFailedException ex)
            {
                Debug.LogException(ex);
            }
        }

        public void OnSignInComplete(string newUserId)
        {
            if (gameData == null || gameData?.PlayerId == "")
            {
                GoogleNewGame?.Invoke(newUserId, false);
                return;
            }
            else
            {
                ShowDetectSaveModal?.Invoke();
            }
        }

        public void OnDenySync()
        {
            GoogleNewGame?.Invoke(currentPlayerId, false);
        }

        public void OnConfirmSync()
        {
            GoogleNewGame?.Invoke(currentPlayerId, true);
        }
    }
}