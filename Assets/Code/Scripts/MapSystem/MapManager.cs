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

        Map gameMap;

        public static MapManager Instance { get; private set; }

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
            InitMapManager();

        }

        void InitMapManager()
        {
            gameMap = LevelManager.Instance.GetMap();
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
                MapViewManager.Instance.GetMapCenter(gameMap.Width, gameMap.Height);
            }
        }
        void DestroyMap()
        {
            for (int i = this.transform.childCount; i > 0; --i)
                DestroyImmediate(this.transform.GetChild(0).gameObject);
        }
    }
}