using System;
using UnityEngine;
using Unity.Game.UI;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Globalization;
using System.Threading;
using System.Net;
using UnityEngine.SceneManagement;

namespace Unity.Game.SaveSystem
{
    public class GameDataManager : MonoBehaviour
    {
        public static event Action NewGameCompleted;

        SaveManager saveManager;

        //[SerializeField] bool isGameDataInitialized;
        [SerializeField] public GameData GameData;
        public static GameDataManager Instance { get; private set; }


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                saveManager = GetComponent<SaveManager>();
            }
            else
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {

            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            //if saved data exists, load saved data
            saveManager?.LoadGame();

            // flag that GameData is loaded the first time
            //isGameDataInitialized = true;
        }

        private void OnEnable()
        {
            NewGameScreenController.LocalNewGame += NewGameWithUserId;
            GoogleSyncScreenController.GoogleNewGame += SyncGameData;

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            NewGameScreenController.LocalNewGame -= NewGameWithUserId;
            GoogleSyncScreenController.GoogleNewGame -= SyncGameData;

            SceneManager.sceneLoaded -= OnSceneLoaded;

        }

        void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            saveManager.InvokeGameDataLoad();
        }

        private void NewGameWithUserId(string playerId)
        {
            GameData = saveManager.NewGame();
            GameData.PlayerId = playerId;
            GameData.Email = "";

            saveManager.SaveGame();
            saveManager.InvokeGameDataLoad();

            NewGameCompleted?.Invoke();
        }

        private async void SyncGameData(string playerId, string playerEmail, bool isSync)
        {
            var apiClient = new APIClient();

            // if is sync is true, it is necessary to send all history first
            if (GameData == null || GameData.PlayerId == null)
            {
                GameData = saveManager.NewGame();
            }

            GameData.PlayerId = playerId;
            GameData.Email = playerEmail;

            if (isSync)
            {
                //Debug.Log("Going Sync");

                // send all history first
                await SendGameData();
            }
            else
            {
                //Debug.Log("Don't Sync");
            }

            // retrieve game data
            await UpdateGameData(playerId);

            NewGameCompleted?.Invoke();
        }

        public async Task UpdateGameData(string playerId)
        {
            if (GameData.PlayerId == "__guest__")
            {
                //Debug.LogWarning("try to update game data as guest");
                return;
            }

            var apiClient = new APIClient();

            try
            {
                GameData newGameData = await apiClient.GetGameData(playerId);
                //Debug.Log($"retrieve game data: {newGameData.SessionHistories} {newGameData.SubmitBest}");
                GameData = newGameData;
            }
            catch (APIException ex)
            {
                Debug.LogErrorFormat("Receive a non successful status code from server while getting game data: {0}", ex.Content);
            }
            catch (HttpRequestException ex)
            {
                Debug.LogErrorFormat("An error occurred while making http request to get game data endpoint: {0}", ex);
            }
            saveManager.SaveGame();
            saveManager.InvokeGameDataLoad();
            NewGameCompleted?.Invoke();

        }

        public async Task SendGameData()
        {
            if (GameData.PlayerId == "__guest__")
            {
                //Debug.LogWarning("try to send game data as guest");
                return;
            }

            // Try send data to backend
            var apiClient = new APIClient();

            // first check if client can connect to backend service
            bool haveConnectionToServer = await apiClient.ConnectionCheck();
            if (!haveConnectionToServer)
            {
                return;
            }

            // send session history to server
            List<SessionStatus> gameSessionWithSendStatus = GameData.SessionHistories;

            foreach (SessionStatus entry in gameSessionWithSendStatus)
            {
                GameSession gameSession = entry.Session;
                bool isAlreadySend = entry.Status;
                if (isAlreadySend)
                {
                    continue;
                }

                GameSessionHistoryRequest dto = new GameSessionDTOMapper().ToDTO(gameSession);

                try
                {
                    await apiClient.SendSessionHistoryData(GameData.PlayerId, dto);
                    entry.Status = true;
                }
                catch (APIException ex)
                {
                    Debug.LogErrorFormat("Receive a non successful status code from server while sending session history: {0}", ex.Content);
                    if (ex.Response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        continue;
                    }
                    break;
                }
                catch (HttpRequestException ex)
                {
                    Debug.LogErrorFormat("An error occurred while making http request to create session history endpoint: {0}", ex);
                    break;
                }
            }

            // send submit best to server
            Dictionary<long, SubmitHistory> submitBest = GameData.SubmitBest;
            var topSubmitHistoryRequests = new List<TopSubmitHistoryRequest>();
            var submitHistoryDTOMapper = new SubmitHistoryDTOMapper();

            foreach (KeyValuePair<long, SubmitHistory> entry in submitBest)
            {
                long mapId = entry.Key;
                SubmitHistory submitHistory = entry.Value;

                var topSubmitHistoryRequest = new TopSubmitHistoryRequest
                {
                    MapId = mapId,
                    TopSubmitHistory = submitHistoryDTOMapper.ToDTO(submitHistory)
                };

                topSubmitHistoryRequests.Add(topSubmitHistoryRequest);
            }

            try
            {
                await apiClient.SendTopSubmitHistory(GameData.PlayerId, topSubmitHistoryRequests);
            }
            catch (APIException ex)
            {
                Debug.LogErrorFormat("Receive a non successful status code from server while sending top submit history: {0}", ex.Content);
            }
            catch (HttpRequestException ex)
            {
                Debug.LogErrorFormat("An error occurred while making http request to update top submit history endpoint: {0}", ex);
            }

        }


    }
}