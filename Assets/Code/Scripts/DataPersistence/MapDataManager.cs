using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using GlobalConfig;
using Unity.Game.MapSystem;
using Unity.Game.UI;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http;

namespace Unity.Game.SaveSystem
{
    public class MapDataManager : MonoBehaviour
    {
        [SerializeField] GameDataManager gameDataManager;
        [SerializeField] MapEntryManager mapEntryManager;
        [SerializeField] string m_MapFilename = "mapdata.dat";

        public static event Action<List<WorldData>> WorldDataLoaded;

        private void Awake()
        {
            if (gameDataManager == null)
            {
                GameObject gameDataManagerGameObject = GameObject.Find("GameDataManager");
                gameDataManager = gameDataManagerGameObject.GetComponent<GameDataManager>();
            }
        }

        private void Start()
        {

        }

        private void OnEnable()
        {
            //MapEntryManager.LoadMap += OnLoadMap;
        }

        private void OnDisable()
        {
            //MapEntryManager.LoadMap -= OnLoadMap;
        }

        public async void OnLoadMap()
        {
            // load map data from files
            List<WorldData> worldDatas = new List<WorldData>();

            if (FileManager.LoadFromFile(m_MapFilename, out var jsonString))
            {
                //Debug.Log("Load String From Files: "+jsonString);
                worldDatas = LoadJson(jsonString);
                // notify other game objects 
                WorldDataLoaded?.Invoke(worldDatas);
            }
            else
            {
                await UpdateMap();
            }
            
           

        }

        public void SaveMap()
        {
            // TODO Save WorldData to files
            string jsonFile = ToJson(mapEntryManager.WorldDatas);

            // save to disk with FileDataHandler
            if (FileManager.WriteToFile(m_MapFilename, jsonFile))
            {
                Debug.Log("MapDataManager.SaveMapData: " + m_MapFilename + " json string: " + jsonFile);
            }
        }

        public async Task UpdateMap()
        {
            string playerId = gameDataManager.GameData.PlayerId;
            if (playerId == "__guest__")
            {
                TextAsset jsonString = Resources.Load<TextAsset>("GuestMapData");
                List<WorldData> guestWorldDatas = LoadJson(jsonString.text);
                MapImageManager.DeleteAllMapImages();
                WorldDataLoaded?.Invoke(guestWorldDatas);
                SaveMap();
                return;
            }

            var apiClient = new APIClient();
            bool haveConnectionToServer = await apiClient.ConnectionCheck();
            if (!haveConnectionToServer)
            {
                return;
            }

            List<WorldData> worldDatas = await apiClient.GetMapData(playerId);
            Debug.Log($"update world data count {worldDatas.Count}");
            MapImageManager.DeleteAllMapImages();
            WorldDataLoaded?.Invoke(worldDatas);
            SaveMap();
        }

        public string ToJson(List<WorldData> worldDatas)
        {
            return JsonConvert.SerializeObject(worldDatas);
        }

        public static List<WorldData> LoadJson(string jsonString)
        {
            return JsonConvert.DeserializeObject<List<WorldData>>(jsonString);
        }

    }
}