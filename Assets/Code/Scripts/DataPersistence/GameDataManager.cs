using System;
using UnityEngine;
using Unity.Game.UI;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Globalization;
using System.Threading;
using System.Net;

namespace Unity.Game.SaveSystem
{
    public class GameDataManager : MonoBehaviour
    {
        public static event Action NewGameCompleted;

        SaveManager saveManager;

        [SerializeField] bool isGameDataInitialized;
        [SerializeField] GameData gameData;
        public GameData GameData { set => gameData = value; get => gameData; }

        private void Awake()
        {
            saveManager = GetComponent<SaveManager>();
        }

        private void Start()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            //if saved data exists, load saved data
            saveManager?.LoadGame();

            // flag that GameData is loaded the first time
            isGameDataInitialized = true;
        }

        private void OnEnable()
        {
            NewGameScreenController.LocalNewGame += NewGameWithUserId;
            GoogleSyncScreenController.GoogleNewGame += SyncGameData;
        }

        private void OnDisable()
        {
            NewGameScreenController.LocalNewGame -= NewGameWithUserId;
            GoogleSyncScreenController.GoogleNewGame -= SyncGameData;

        }

        private void NewGameWithUserId(string playerId)
        {
            gameData = saveManager.NewGame();
            gameData.PlayerId = playerId;

            saveManager.SaveGame();
            saveManager.InvokeGameDataLoad();

            NewGameCompleted?.Invoke();
        }

        private async void SyncGameData(string playerId, bool isSync)
        {
            var apiClient = new APIClient();

            // if is sync is true, it is necessary to send all history first
            if (isSync)
            {
                Debug.Log("Going Sync");

                // send all history first
                await SendGameData();
            }
            else
            {
                Debug.Log("Don't Sync");
            }

            // retrieve game data
            await UpdateGameData(playerId);
           
            NewGameCompleted?.Invoke();
        }

        public async Task UpdateGameData(string playerId)
        {
            if (gameData.PlayerId == "__guest__")
            {
                Debug.LogWarning("try to update game data as guest");
                return;
            }

            var apiClient = new APIClient();

            try
            {
                GameData newGameData = await apiClient.GetGameData(playerId);
                Debug.Log($"retrieve game data: {newGameData.SessionHistories} {newGameData.SubmitBest}");
                gameData = newGameData;
            }
            catch (APIException ex)
            {
                Debug.LogErrorFormat("Receive a non successful status code from server while getting game data: {0}", ex.Content);
            }
            catch (HttpRequestException ex)
            {
                Debug.LogErrorFormat("An error occurred while making http request to get game data endpoint: {0}", ex);
            }
        }

        public async Task SendGameData()
        {
            if (gameData.PlayerId == "__guest__")
            {
                Debug.LogWarning("try to send game data as guest");
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
            List<SessionStatus> gameSessionWithSendStatus = gameData.SessionHistories;

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
                    await apiClient.SendSessionHistoryData(gameData.PlayerId, dto);
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
            Dictionary<long, SubmitHistory> submitBest = gameData.SubmitBest;
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
                await apiClient.SendTopSubmitHistory(gameData.PlayerId, topSubmitHistoryRequests);
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