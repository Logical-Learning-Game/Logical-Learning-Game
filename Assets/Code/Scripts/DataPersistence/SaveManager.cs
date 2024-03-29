using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using GlobalConfig;
using Unity.Game.MapSystem;
using UnityEngine.SceneManagement;
using System.Threading;
using System.Globalization;

namespace Unity.Game.SaveSystem
{
    public class SaveManager : MonoBehaviour
    {
        
        [SerializeField] string m_SaveFilename = "savegame.dat";
        GameDataManager gameDataManager;
        public static event Action<GameData> GameDataLoaded;

        public static SaveManager Instance { get; private set; }
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                gameDataManager = GetComponent<GameDataManager>();
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
        }

        private void OnDestroy()
        {
            if (gameDataManager != null)
            {
                SaveGame();
            }
            
        }

        public void InvokeGameDataLoad()
        {
            GameDataLoaded?.Invoke(gameDataManager.GameData);
        }

        //void OnApplicationQuit()
        //{
        //    SaveGame();
        //}



        public void LoadGame()
        {
            // load saved data from FileDataHandler

            //if (gameDataManager.GameData == null)
            //{
            //    Debug.Log("GameData is null");
            //    gameDataManager.GameData = NewGame();
            //}
            //else 

            if (FileManager.LoadFromFile(m_SaveFilename, out var jsonString))
            {
                //Debug.Log("loadData from file:"+jsonString);
                gameDataManager.GameData = GameData.LoadJson(jsonString);
            }
            //else
            //{
            //    gameDataManager.GameData = NewGame();
            //}
            // notify other game objects 
            if (gameDataManager.GameData != null)
            {
                GameDataLoaded?.Invoke(gameDataManager.GameData);
            }
        }

        public void SaveGame()
        {
            string jsonFile = gameDataManager.GameData.ToJson();

            // save to disk with FileDataHandler
            if (FileManager.WriteToFile(m_SaveFilename, jsonFile))
            {
                //Debug.Log("SaveManager.SaveGame: " + m_SaveFilename + " json string: " + jsonFile);
            }
        }

        public GameData NewGame()
        {
            return new GameData();
        }
    }
}