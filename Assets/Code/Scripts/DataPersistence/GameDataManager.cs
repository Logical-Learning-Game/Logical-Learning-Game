using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using GlobalConfig;
using Unity.Game.MapSystem;

namespace Unity.Game.SaveSystem
{
    public class GameDataManager : MonoBehaviour
    {
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

        void AddSubmit(Submit submit)
        {
            // implement add submit to List
            // if have the internet
            if (true)
            {
                Debug.Log("Adding Submit");
                // send submit to internet
                gameData.SubmitHistory.Add(submit, true);
            }
            else
            {
                gameData.SubmitHistory.Add(submit, false);
            }

            //if this submit is new high score
            //gameData.SubmitBest.TryAdd(submit.mapId, submit);


            saveManager.SaveGame();
        }

    }
}