using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using GlobalConfig;
using Unity.Game.MapSystem;
using Unity.Game.UI;

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

        void AddSubmit(Submit submit)
        {
            // implement add submit to List
            Debug.Log("Adding Submit to current game session");
            SessionManager.CurrentGameSession.SubmitHistories.Add(submit);


            //if this submit is new high score
            //gameData.SubmitBest.TryAdd(submit.mapId, submit);


            saveManager.SaveGame();
        }

        void NewGameWithUserId(string UserId)
        {
            gameData = saveManager.NewGame();
            gameData.UserId = UserId;

            NewGameCompleted?.Invoke();
        }

        void SyncGameData(string UserId, bool isSync)
        {
            // if is sync is true, it is necessary to send all history first
            if (isSync)
            {
                Debug.Log("Going Sync");
            }
            else
            {
                Debug.Log("Don't Sync");
            }

            //and then , load history back from backend

            gameData.UserId = UserId;
            NewGameCompleted?.Invoke();

        }
    }
}