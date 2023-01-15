using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalConfig;
using Unity.Game.Conditions;

namespace Unity.Game.Map
{

    [ExecuteInEditMode]
    public class MapManager : MonoBehaviour
    {

        [SerializeField] private GameObject EmptyTile;
        [SerializeField] private GameObject ObstracleTile;
        [SerializeField] private GameObject GoalTile;
        [SerializeField] private GameObject DoorTile;
        [SerializeField] private GameObject ConditionTile;

        public Map gameMap = new Map();

        public static MapManager Instance { get; private set; }
        // temporary map string
        // E = Empty
        // O = Obstacle
        // C = Condition
        // G = Goal
        // D = Door

        private string[,] textTiles = new string[,]
        {
           { "E","O","O","G" },
           { "E","O","O","D" },
           { "E","O","O","D" },
           { "E","E","E","E" },
           //{ "E","O","O","G","O","E" },
           //{ "E","O","O","D","O","E" },
           //{ "E","O","O","D","O","E" },
           //{ "E","E","E","E","O","E" },
           //{ "E","E","E","E","O","E" },
        };

        public GameObject[,] TileObjects;
        // Start is called before the first frame update
        void Awake()
        {
            Debug.Log("MapManager Awake");
            if (Instance == null)
            {
                Instance = this;

            }
            else
            {
                Destroy(gameObject);
            }

        }

        private void Start()
        {
            DestroyMap();
            CreateMap();
        }

        // Update is called once per frame
        void Update()
        {
            //briefly test OnTileEntered and IsEnterable
            if (Input.GetKeyDown(KeyCode.T))
            {
                foreach (GameObject tile in TileObjects)
                {
                    if (tile.GetComponent<Tile>().IsEnterable())
                    {
                        tile.GetComponent<Tile>().OnTileEntered();
                    }
                }
            }
        }

        // old createmap using string array
        //void CreateMap()
        //{
        //    TileObjects = new GameObject[textTiles.GetLength(0), textTiles.GetLength(1)];
        //    for (int i = 0; i < textTiles.GetLength(0); i++)
        //    {
        //        for (int j = 0; j < textTiles.GetLength(1); j++)
        //        {
        //            switch (textTiles[i, j])
        //            {
        //                case "E":
        //                    TileObjects[i, j] = Instantiate(EmptyTile, new Vector3(i * MapConfig.TILE_SCALE, 0, j * MapConfig.TILE_SCALE), Quaternion.identity);
        //                    TileObjects[i, j].transform.SetParent(transform);
        //                    break;
        //                case "O":
        //                    TileObjects[i, j] = Instantiate(ObstracleTile, new Vector3(i * MapConfig.TILE_SCALE, 0, j * MapConfig.TILE_SCALE), Quaternion.identity);
        //                    TileObjects[i, j].transform.SetParent(transform);
        //                    break;
        //                case "G":
        //                    TileObjects[i, j] = Instantiate(GoalTile, new Vector3(i * MapConfig.TILE_SCALE, 0, j * MapConfig.TILE_SCALE), Quaternion.identity);
        //                    TileObjects[i, j].transform.SetParent(transform);
        //                    break;
        //                case "D":
        //                    TileObjects[i, j] = Instantiate(DoorTile, new Vector3(i * MapConfig.TILE_SCALE, 0, j * MapConfig.TILE_SCALE), Quaternion.identity);
        //                    TileObjects[i, j].transform.SetParent(transform);
        //                    break;
        //                case "C":
        //                    TileObjects[i, j] = Instantiate(ConditionTile, new Vector3(i * MapConfig.TILE_SCALE, 0, j * MapConfig.TILE_SCALE), Quaternion.identity);
        //                    TileObjects[i, j].transform.SetParent(transform);
        //                    break;
        //                default:
        //                    break;
        //            }
        //        }
        //    }

        //    if (Application.isPlaying)
        //    {
        //        MapViewManager.Instance.GetMapCenter(textTiles.GetLength(0), textTiles.GetLength(1));
        //    }
        //}

        void CreateMap()
        {
            TileType[,] tileArray = gameMap.TileArray;
            TileObjects = new GameObject[gameMap.Width, gameMap.Height];
            for (int i = 0; i < gameMap.Width; i++)
            {
                for (int j = 0; j < gameMap.Height; j++)
                {
                    switch (gameMap.TileArray[i, j])
                    {
                        case TileType.EMPTY:
                            TileObjects[i, j] = Instantiate(EmptyTile, new Vector3(i * MapConfig.TILE_SCALE, 0, j * MapConfig.TILE_SCALE), Quaternion.identity);
                            TileObjects[i, j].transform.SetParent(transform);
                            break;
                        case TileType.OBSTACLE:
                            TileObjects[i, j] = Instantiate(ObstracleTile, new Vector3(i * MapConfig.TILE_SCALE, 0, j * MapConfig.TILE_SCALE), Quaternion.identity);
                            TileObjects[i, j].transform.SetParent(transform);
                            break;
                        case TileType.GOAL:
                            TileObjects[i, j] = Instantiate(GoalTile, new Vector3(i * MapConfig.TILE_SCALE, 0, j * MapConfig.TILE_SCALE), Quaternion.identity);
                            TileObjects[i, j].transform.SetParent(transform);
                            break;
                        case TileType.DOOR:
                            TileObjects[i, j] = Instantiate(DoorTile, new Vector3(i * MapConfig.TILE_SCALE, 0, j * MapConfig.TILE_SCALE), Quaternion.identity);
                            TileObjects[i, j].transform.SetParent(transform);
                            break;
                        case TileType.CONDITION_A:
                            TileObjects[i, j] = Instantiate(ConditionTile, new Vector3(i * MapConfig.TILE_SCALE, 0, j * MapConfig.TILE_SCALE), Quaternion.identity);
                            TileObjects[i, j].GetComponent<ConditionTile>().SetTileCondition(ConditionSign.A);
                            TileObjects[i, j].transform.SetParent(transform);
                            break;
                        case TileType.CONDITION_B:
                            TileObjects[i, j] = Instantiate(ConditionTile, new Vector3(i * MapConfig.TILE_SCALE, 0, j * MapConfig.TILE_SCALE), Quaternion.identity);
                            TileObjects[i, j].GetComponent<ConditionTile>().SetTileCondition(ConditionSign.B);
                            TileObjects[i, j].transform.SetParent(transform);
                            break;
                        case TileType.CONDITION_C:
                            TileObjects[i, j] = Instantiate(ConditionTile, new Vector3(i * MapConfig.TILE_SCALE, 0, j * MapConfig.TILE_SCALE), Quaternion.identity);
                            TileObjects[i, j].GetComponent<ConditionTile>().SetTileCondition(ConditionSign.C);
                            TileObjects[i, j].transform.SetParent(transform);
                            break;
                        case TileType.CONDITION_D:
                            TileObjects[i, j] = Instantiate(ConditionTile, new Vector3(i * MapConfig.TILE_SCALE, 0, j * MapConfig.TILE_SCALE), Quaternion.identity);
                            TileObjects[i, j].GetComponent<ConditionTile>().SetTileCondition(ConditionSign.D);
                            TileObjects[i, j].transform.SetParent(transform);
                            break;
                        case TileType.CONDITION_E:
                            TileObjects[i, j] = Instantiate(ConditionTile, new Vector3(i * MapConfig.TILE_SCALE, 0, j * MapConfig.TILE_SCALE), Quaternion.identity);
                            TileObjects[i, j].GetComponent<ConditionTile>().SetTileCondition(ConditionSign.E);
                            TileObjects[i, j].transform.SetParent(transform);
                            break;
                        default:
                            break;
                    }
                }
            }

            if (Application.isPlaying)
            {
                MapViewManager.Instance.GetMapCenter(textTiles.GetLength(0), textTiles.GetLength(1));
            }
        }
        void DestroyMap()
        {
            for (int i = this.transform.childCount; i > 0; --i)
                DestroyImmediate(this.transform.GetChild(0).gameObject);
        }
    }
}