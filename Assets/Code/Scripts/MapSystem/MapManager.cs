using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalConfig;
using Unity.Game.Conditions;
using Unity.Game.Level;
using Unity.Game.ItemSystem;

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

        public Dictionary<GameObject, int[,]> DoorObjects;
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
            DoorObjects = new Dictionary<GameObject, int[,]>();
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
                    InitDoor(i, j, shifted);

                }
            }



            if (Application.isPlaying)
            {
                CreateDoors();
                MapViewManager.Instance.GetMapCenter(gameMap.Width, gameMap.Height);
            }
        }
        void DestroyMap()
        {
            for (int i = this.transform.childCount; i > 0; --i)
                DestroyImmediate(this.transform.GetChild(0).gameObject);
        }

        void InitDoor(int tileI, int tileJ, uint remainder)
        {

            Dictionary<int, Tuple<int, int>> dict = new Dictionary<int, Tuple<int, int>>()
            {
                { 0, Tuple.Create(0, 1) },
                { 1, Tuple.Create(1, 0) },
                { 2, Tuple.Create(0, -1) },
                { 3, Tuple.Create(-1, 0) },
            };
            // i is x ,j is z
            //shift through item data
            remainder >>= 4;
            if (remainder == 0) return;
            int directionCounter = 0;

            while (remainder != 0)
            {

                if ((remainder & 0b1) == 1)
                {
                    Debug.Log("Found Door At (" + tileI + "," + tileJ + ") to " +"("+ (tileI + dict[directionCounter].Item1) + ", " + (tileJ + dict[directionCounter].Item2) + ")");
                    
                    Debug.Log("Door Type is"+((remainder>>1)&0b11));
                   
                }
                directionCounter++;
                remainder >>= 4;
            }
            // this function is not complete

        }

        void CreateDoors()
        {

        }
    }
}