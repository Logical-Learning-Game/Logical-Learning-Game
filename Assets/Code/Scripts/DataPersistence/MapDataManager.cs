using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using GlobalConfig;
using Unity.Game.MapSystem;
using Unity.Game.UI;
using Newtonsoft.Json;

namespace Unity.Game.SaveSystem
{
    public class MapDataManager : MonoBehaviour
    {

        [SerializeField] MapEntryManager mapEntryManager;
        [SerializeField] string m_MapFilename = "mapdata.dat";

        public static event Action<List<WorldData>> WorldDataLoaded;

        private void Awake()
        {
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

        public List<WorldData> OnLoadMap()
        {
            // load map data from files
            List<WorldData> WorldDatas = new List<WorldData>();

            if (FileManager.LoadFromFile(m_MapFilename, out var jsonString))
            {
                Debug.Log("Load String From Files: "+jsonString);
                WorldDatas = LoadJson(jsonString);
                // notify other game objects 
            }
            
            WorldDataLoaded?.Invoke(WorldDatas);
            return WorldDatas;

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

        public void UpdateMap()
        {

        }

        public string ToJson(List<WorldData> worldDatas)
        {
            //return JsonUtility.ToJson(this);
            return JsonConvert.SerializeObject(worldDatas);
        }

        public static List<WorldData> LoadJson(string jsonString)
        {
            //JsonUtility.FromJsonOverwrite(jsonFilepath, this);
            return JsonConvert.DeserializeObject<List<WorldData>>(jsonString);
        }

    }
}