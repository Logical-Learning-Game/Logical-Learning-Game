using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Game.Level;
using Unity.Game.MapSystem;

namespace Unity.Game.ItemSystem
{

    public class ItemManager : MonoBehaviour
    {

        public static ItemManager Instance { get; private set; }
        Map gameMap;

        public List<GameObject> ItemList;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            InitItemManager();
        }

        void InitItemManager()
        {
            if (ItemList.Count > 0)
            {
                ItemList.Clear();
                DestroyAllItemObjects();
            }
            gameMap = LevelManager.Instance.GetMap();
            SpawnItemsInMap();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void SpawnItemsInMap()
        {

        }

        void RemoveItemFromMap(Item item)
        {

        }

        void DestroyAllItemObjects()
        {

            foreach (GameObject child in ItemList)
            {
                Destroy(child);
            }

        }
    }

}