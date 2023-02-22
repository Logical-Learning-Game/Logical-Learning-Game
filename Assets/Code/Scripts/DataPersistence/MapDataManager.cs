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
    public class MapDataManager : MonoBehaviour
    {


        [SerializeField] string m_MapFilename = "Map.dat";

        public static event Action<Dictionary<string, List<Map>>> MapDataLoaded;

        private void Awake()
        {
        }

        private void Start()
        {

        }


        public void LoadMap()
        {
            // load map data from files

            if (MapEntryManager.MapLists == null)
            {
                //gameDataManager.GameData = NewGame();
            }
            else if (FileManager.LoadFromFile(m_MapFilename, out var jsonString))
            {
                Debug.Log(jsonString);
            }

            // notify other game objects 
            if (MapEntryManager.MapLists != null)
            {
                MapDataLoaded?.Invoke(MapEntryManager.MapLists);
            }
        }


    }
}