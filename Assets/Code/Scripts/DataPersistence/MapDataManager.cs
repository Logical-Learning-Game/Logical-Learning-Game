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
        [SerializeField] MapEntryManager mapEntryManager;
        [SerializeField] string m_MapFilename = "mapdata.dat";

        public static event Action<List<WorldData>> WorldDataLoaded;

        public bool isFetching = false;
        private void Awake()
        {

        }

        private void Start()
        {

        }

        private void OnEnable()
        {
            GameDataManager.NewGameCompleted += ReloadMapData;
            //MapEntryManager.LoadMap += OnLoadMap;
        }

        private void OnDisable()
        {
            GameDataManager.NewGameCompleted -= ReloadMapData;
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

        public async void ReloadMapData()
        {
            Debug.Log("Reload Map");
            await UpdateMap();
        }

        public async Task UpdateMap()
        {


            if (GameDataManager.Instance.GameData == null || GameDataManager.Instance?.GameData?.PlayerId == null)
            {
                Debug.Log("No map, and player is not starting game yet");
                return;
            }
            if (isFetching) return;
            isFetching = true;

            Debug.Log($"Player Id : {GameDataManager.Instance.GameData.PlayerId}");
            if (GameDataManager.Instance.GameData.PlayerId == "__guest__" )
            {
                Debug.Log("Player is Guest, Loading Mapdata from Resources");
                TextAsset jsonString = Resources.Load<TextAsset>("GuestMapData");
                List<WorldData> guestWorldDatas = LoadJson(jsonString.text);
                MapImageManager.DeleteAllMapImages();
                WorldDataLoaded?.Invoke(guestWorldDatas);
                SaveMap();
                isFetching = false;
                return;
            }

            var apiClient = new APIClient();
            bool haveConnectionToServer = await apiClient.ConnectionCheck();
            if (!haveConnectionToServer)
            {
                // TODO
                isFetching = false;
                return;
            }

            List<WorldData> worldDatas = await apiClient.GetMapData(GameDataManager.Instance.GameData.PlayerId);
            Debug.Log("Loading MapData From Backend");
            MapImageManager.DeleteAllMapImages();
            WorldDataLoaded?.Invoke(worldDatas);
            SaveMap();

            isFetching = false;
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