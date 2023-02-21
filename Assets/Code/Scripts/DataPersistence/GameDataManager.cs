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
    public class GameDataManager: MonoBehaviour
    {
        SaveManager saveManager;

        bool isGameDataInitialized;
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
            //Submit submit = new Submit("test", "testmap", "testsession", new List<CommandPattern>(), new List<CommandEdge>(), true, true, new List<RuleSystem.Rule>(), new bool[] { true, true, true }, Medal.NONE, Medal.NONE, new StateValue());
            
            //AddSubmit(submit);
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

    }
}