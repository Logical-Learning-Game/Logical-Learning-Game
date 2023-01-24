using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalConfig;
using Unity.Game.Conditions;
using Unity.Game.Level;

namespace Unity.Game.MapSystem
{

    [ExecuteInEditMode]
    public class MapManager : MonoBehaviour
    {

        [SerializeField] private GameObject EmptyTile;
        [SerializeField] private GameObject ObstracleTile;
        [SerializeField] private GameObject GoalTile;
        [SerializeField] private GameObject DoorTile;
        [SerializeField] private GameObject ConditionTile;

        public static MapManager Instance { get; private set; }

        public GameObject[,] TileObjects;
        // Start is called before the first frame update
        void Awake()
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

        void Start()
        {


        }

        public void InitMap()
        {
            Map gameMap = LevelManager.Instance.GetMap();
            DestroyMap();
            CreateMap(gameMap);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void CreateMap(Map gameMap)
        {
            uint[,] MapArray = gameMap.MapData;
            TileObjects = new GameObject[gameMap.Width, gameMap.Height];
            for (int i = 0; i < gameMap.Width; i++)
            {
                for (int j = 0; j < gameMap.Height; j++)
                {
                    uint shifted = MapArray[i, j] >> 4;
                    switch (shifted & 0b1111)
                    {
                        case 0b0000: // empty
                            TileObjects[i, j] = Instantiate(EmptyTile, new Vector3(i * MapConfig.TILE_SCALE, 0, j * MapConfig.TILE_SCALE), Quaternion.identity);
                            TileObjects[i, j].transform.SetParent(transform);
                            break;
                        case 0b0001: //obstacle
                            TileObjects[i, j] = Instantiate(ObstracleTile, new Vector3(i * MapConfig.TILE_SCALE, 0, j * MapConfig.TILE_SCALE), Quaternion.identity);
                            TileObjects[i, j].transform.SetParent(transform);
                            break;
                        case 0b0010: //goal
                            TileObjects[i, j] = Instantiate(GoalTile, new Vector3(i * MapConfig.TILE_SCALE, 0, j * MapConfig.TILE_SCALE), Quaternion.identity);
                            TileObjects[i, j].transform.SetParent(transform);
                            break;
                        case 0b0011: //cond_a
                            TileObjects[i, j] = Instantiate(ConditionTile, new Vector3(i * MapConfig.TILE_SCALE, 0, j * MapConfig.TILE_SCALE), Quaternion.identity);
                            TileObjects[i, j].GetComponent<ConditionTile>().SetTileCondition(ConditionSign.A);
                            TileObjects[i, j].transform.SetParent(transform);
                            break;
                        case 0b0100: //cond_b
                            TileObjects[i, j] = Instantiate(ConditionTile, new Vector3(i * MapConfig.TILE_SCALE, 0, j * MapConfig.TILE_SCALE), Quaternion.identity);
                            TileObjects[i, j].GetComponent<ConditionTile>().SetTileCondition(ConditionSign.B);
                            TileObjects[i, j].transform.SetParent(transform);
                            break;
                        case 0b0101: //cond_c
                            TileObjects[i, j] = Instantiate(ConditionTile, new Vector3(i * MapConfig.TILE_SCALE, 0, j * MapConfig.TILE_SCALE), Quaternion.identity);
                            TileObjects[i, j].GetComponent<ConditionTile>().SetTileCondition(ConditionSign.C);
                            TileObjects[i, j].transform.SetParent(transform);
                            break;
                        case 0b0110: //cond_d
                            TileObjects[i, j] = Instantiate(ConditionTile, new Vector3(i * MapConfig.TILE_SCALE, 0, j * MapConfig.TILE_SCALE), Quaternion.identity);
                            TileObjects[i, j].GetComponent<ConditionTile>().SetTileCondition(ConditionSign.D);
                            TileObjects[i, j].transform.SetParent(transform);
                            break;
                        case 0b0111: //cond_e
                            TileObjects[i, j] = Instantiate(ConditionTile, new Vector3(i * MapConfig.TILE_SCALE, 0, j * MapConfig.TILE_SCALE), Quaternion.identity);
                            TileObjects[i, j].GetComponent<ConditionTile>().SetTileCondition(ConditionSign.E);
                            TileObjects[i, j].transform.SetParent(transform);
                            break;
                        default:
                            break;
                    }
                    InitDoor(TileObjects[i, j], shifted);
                    
                }
            }

            if (Application.isPlaying)
            {
                MapViewManager.Instance.GetMapCenter(gameMap.Width, gameMap.Height);
            }
        }
        void DestroyMap()
        {
            for (int i = this.transform.childCount; i > 0; --i)  
               DestroyImmediate(this.transform.GetChild(0).gameObject);
        }
        
        void InitDoor(GameObject tileObject,uint remainder)
        {
            //shift through item data
            if (remainder>>4 == 0) return;
            int direction = 0;
            // init door from each side, implement later
            //while(remainder != 0)
            //{
            //    remainder = remainder >> 4;
            //}
        }
    }
}