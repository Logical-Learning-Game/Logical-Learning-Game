using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using GlobalConfig;
using Unity.Game.MapSystem;

namespace Unity.Game.SaveSystem
{
    public class MapDataManager : MonoBehaviour
    {

        Dictionary<string, List<Map>> MapLists = new Dictionary<string, List<Map>>();
        Dictionary<string, List<GameObject>> MapInstanceLists = new Dictionary<string, List<GameObject>>();
        private void Awake()
        {

        }

        private void Start()
        {

        }

        public void UpdateMapData()
        {
            // mockup for sync map data with network
        }
        
        public void LoadMapFromFile()
        {
            // read map from file
        }
        
        public List<GameObject> GetMapDisplayList(string worldSelector)
        {
            MapInstanceLists.TryGetValue(worldSelector, out List<GameObject> MapInstances);
            if(MapInstances is null)
            {
                MapLists.TryGetValue(worldSelector, out List<Map> MapData);
                MapInstances = new List<GameObject>();
                foreach(Map map in MapData)
                {
                    MapInstances.Add(GetMapDisplay(map));
                }
                MapInstanceLists.TryAdd(worldSelector, MapInstances);
            }
            
            return MapInstances;
        }
        
        public List<string> GetWorldInstance()
        {
            return MapLists.Keys.ToList();
        }

        public GameObject GetMapDisplay(Map map)
        {
            GameObject MapDisplay = Instantiate(new GameObject());
            // assign map value to mapDisplay

            return MapDisplay;
        }

    }
}
