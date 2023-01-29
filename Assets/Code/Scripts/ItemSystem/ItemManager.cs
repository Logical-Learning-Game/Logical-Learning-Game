using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Game.Level;
using Unity.Game.MapSystem;
using GlobalConfig;

namespace Unity.Game.ItemSystem
{

    public class ItemManager : MonoBehaviour
    {
        [SerializeField] private GameObject KeyA;
        [SerializeField] private GameObject KeyB;
        [SerializeField] private GameObject KeyC;
        public static ItemManager Instance { get; private set; }

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
    
        }

        public void InitItems()
        {
            if (ItemList.Count > 0)
            {
                DestroyAllItemObjects();
                ItemList.Clear();
            }
            Map gameMap = LevelManager.Instance.GetMap();
            SpawnItemsInMap(gameMap);
        }

        // Update is called once per frame
        void Update()
        {

        }
        
        void SpawnItemsInMap(Map gameMap)
        {
            uint[,] MapArray = gameMap.MapData;

            for (int i = 0; i < gameMap.Width; i++)
            {
                for (int j = 0; j < gameMap.Height; j++)
                {
                    uint shifted = MapArray[i, j] >> 8;
                    GameObject ItemObject;
                    switch (shifted & 0b1111)
                    {
                        case 0b0000: // no item
                            break;
                        case 0b0001: // Key_A
                            ItemObject = Instantiate(KeyA, new Vector3(i * MapConfig.TILE_SCALE, 2, j * MapConfig.TILE_SCALE), Quaternion.identity);
                            ItemList.Add(ItemObject);
                            MapManager.Instance.TileObjects[i, j].GetComponent<Tile>().SetItemObject(ItemObject);
                            ItemObject.transform.SetParent(transform);
                            break;
                        case 0b0010: // Key_B
                            ItemObject = Instantiate(KeyB, new Vector3(i * MapConfig.TILE_SCALE, 2, j * MapConfig.TILE_SCALE), Quaternion.identity);
                            ItemList.Add(ItemObject);
                            MapManager.Instance.TileObjects[i, j].GetComponent<Tile>().SetItemObject(ItemObject);
                            ItemObject.transform.SetParent(transform);
                            break;
                        case 0b0011: // Key_C
                            ItemObject = Instantiate(KeyC, new Vector3(i * MapConfig.TILE_SCALE, 2, j * MapConfig.TILE_SCALE), Quaternion.identity);
                            ItemList.Add(ItemObject);
                            MapManager.Instance.TileObjects[i, j].GetComponent<Tile>().SetItemObject(ItemObject);
                            ItemObject.transform.SetParent(transform);
                            break;
                        default:
                            break;
                    }


                }
            }
        }

        public void RemoveItemFromMap(Item item)
        {
            ItemList.Remove(item.gameObject);
            Destroy(item.gameObject);
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