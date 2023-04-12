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
        public static event Action<string, string, bool> GoogleNewGame;

        [SerializeField] bool IsWaitingForSync = false;

        [SerializeField] GameData gameData;
        [SerializeField] string currentPlayerId;
        [SerializeField] string currentPlayerEmail;

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
            GoogleSyncScreen.CancelSyncClick += OnCancelSync;

            SettingPanelManager.GoogleSyncClick += OnGoogleSignIn;
        }

        void OnDisable()
        {
            SaveManager.GameDataLoaded -= OnGameDataLoaded;

            NewGameScreenController.GoogleNewGame -= OnGoogleSignIn;
            GoogleSyncScreen.DenySyncClick -= OnDenySync;
            GoogleSyncScreen.ConfirmSyncClick -= OnConfirmSync;
            GoogleSyncScreen.CancelSyncClick -= OnCancelSync;

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

        public async void OnGoogleSignIn()
        {
            // google signin
            try
            {
                IsWaitingForSync = true;
                if (GoogleAuthenticationManager.Instance == null)
                {
                    gameObject.AddComponent<GoogleAuthenticationManager>();
                }
                (string playerId, string playerEmail) = await GoogleAuthenticationManager.Instance.GoogleSignIn();
                //Debug.Log($"PlayerId: {playerId}");
                currentPlayerId = playerId;
                currentPlayerEmail = playerEmail;

                var apiClient = new APIClient();

                // link account at server
                bool accountLinked = await apiClient.AccountCheck(playerId);
                if (!accountLinked)
                {
                    var linkAccountRequest = new LinkAccountRequest
                    {
                        PlayerId = playerId,
                        Email = playerEmail
                    };
                    await apiClient.LinkAccount(linkAccountRequest);
                }
                OnSignInComplete(currentPlayerId, currentPlayerEmail);
            }
            catch (WebException ex)
            {
                IsWaitingForSync = false;
                Debug.LogException(ex);
            }
            catch (AuthenticationException ex)
            {
                IsWaitingForSync = false;
                Debug.LogException(ex);
            }
            catch (RequestFailedException ex)
            {
                IsWaitingForSync = false;
                Debug.LogException(ex);
            }
        }

        public void OnSignInComplete(string newUserId, string playerEmail = "")
        {
            if (IsWaitingForSync)
            {
                IsWaitingForSync = false;
                if (gameData == null || gameData?.PlayerId == "" || (gameData.SessionHistories.Count == 0 && gameData.SubmitBest.Count == 0))
                {
                    GoogleNewGame?.Invoke(newUserId, playerEmail, false);
                    return;
                }
                else
                {
                    ShowDetectSaveModal?.Invoke();
                }
            }
        }

        public void OnDenySync()
        {
            GoogleNewGame?.Invoke(currentPlayerId,currentPlayerEmail, false);
        }

        public void OnConfirmSync()
        {
            GoogleNewGame?.Invoke(currentPlayerId,currentPlayerEmail, true);
        }

        public void OnCancelSync(string _)
        {
            IsWaitingForSync = false;
        }
    }
}