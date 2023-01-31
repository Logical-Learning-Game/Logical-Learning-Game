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
        [SerializeField] private GameObject DoorPrefab;


        public static MapManager Instance { get; private set; }

        public GameObject[,] TileObjects;

        public List<GameObject> DoorList;
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
            if (!Application.isPlaying)
            {
                Map map = new Map();
                CreateMap(map);
            }

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
      

                }
            }



            if (Application.isPlaying)
            {
                CreateDoors(gameMap);
                MapViewManager.Instance.GetMapCenter(gameMap.Width, gameMap.Height);
            }
        }
        void DestroyMap()
        {
            for (int i = this.transform.childCount; i > 0; --i)
                DestroyImmediate(this.transform.GetChild(0).gameObject);
        }

        //void InitDoor(int tileI, int tileJ, uint remainder)
        //{

        //    Dictionary<int, Tuple<int, int>> dict = new Dictionary<int, Tuple<int, int>>()
        //    {
        //        { 0, Tuple.Create(0, 1) },
        //        { 1, Tuple.Create(1, 0) },
        //        { 2, Tuple.Create(0, -1) },
        //        { 3, Tuple.Create(-1, 0) },
        //    };
        //    // i is x ,j is z
        //    //shift through item data
        //    remainder >>= 8;
        //    if (remainder == 0) return;
        //    int directionCounter = 0;

        //    while (remainder != 0)
        //    {
        //        Debug.Log(directionCounter + ":(" + tileI + "," + tileJ + ") :" + remainder);
        //        if ((remainder & 0b1) == 1)
        //        {
        //            Debug.Log("Found Door At (" + tileI + "," + tileJ + ") to " + "(" + (tileI + dict[directionCounter].Item1) + ", " + (tileJ + dict[directionCounter].Item2) + ")");

        //            //Debug.Log("Door Type is"+((remainder>>1)&0b11));

        //            //Debug.Log("Door Status is" + ((remainder >> 3) & 0b1));

        //            Debug.Log(new Vector3((tileI + dict[directionCounter].Item1 / 2f) * MapConfig.TILE_SCALE, 1.8f, (tileJ + dict[directionCounter].Item2 / 2f) * MapConfig.TILE_SCALE));

        //            GameObject door = Instantiate(DoorPrefab, new Vector3((tileI + dict[directionCounter].Item1 / 2f) * MapConfig.TILE_SCALE, 1.8f, (tileJ + dict[directionCounter].Item2 / 2f) * MapConfig.TILE_SCALE), Quaternion.identity);
        //            door.transform.rotation = Quaternion.Euler(0, dict[directionCounter].Item2 * 90f, 0);
        //        }
        //        directionCounter++;
        //        remainder >>= 4;
        //    }
        //    // this function is not complete

        //}

        void CreateDoors(Map gameMap)
        {
            DoorList = new List<GameObject>();
            Dictionary<int, Tuple<int, int>> dict = new Dictionary<int, Tuple<int, int>>()
            {
                { 0, Tuple.Create(0, 1) },
                { 1, Tuple.Create(1, 0) },
                { 2, Tuple.Create(0, -1) },
                { 3, Tuple.Create(-1, 0) },
            };

            uint[,] MapArray = gameMap.MapData;
            for (int i = 0; i < gameMap.Width; i++)
            {
                for (int j = 0; j < gameMap.Height; j++)
                {
                    uint remainder = MapArray[i, j];
                    remainder >>= 12;
                    if (remainder == 0) continue;
                    int directionCounter = 0;
                    while (remainder != 0)
                    {
                        if ((remainder & 0b1) == 1)
                        {
                            Tuple<int, int> fromPos = Tuple.Create(i, j);
                            Tuple<int, int> toPos = Tuple.Create(i + dict[directionCounter].Item1, j + dict[directionCounter].Item2);
                            Tile fromTile = GetMapTile(fromPos);
                            Tile toTile = GetMapTile(toPos);
                            if (fromTile != null && toTile != null && fromTile.GetDoorOnTile(dict[directionCounter]) == null)
                            {
                                GameObject door = Instantiate(DoorPrefab, new Vector3((i + dict[directionCounter].Item1 / 2f) * MapConfig.TILE_SCALE, 1.8f, (j + dict[directionCounter].Item2 / 2f) * MapConfig.TILE_SCALE), Quaternion.identity);
                                switch ((remainder >> 1) & 0b11)
                                {
                                    case 0b00:
                                        door.GetComponent<Door>().SetDoorKey(ItemType.NONE);
                                        door.GetComponent<Door>().SetDoorGlyph("");
                                        break;
                                    case 0b01:
                                        door.GetComponent<Door>().SetDoorKey(ItemType.KEY_A);
                                        door.GetComponent<Door>().SetDoorGlyph("F");
                                        break;
                                    case 0b10:
                                        door.GetComponent<Door>().SetDoorKey(ItemType.KEY_B);
                                        door.GetComponent<Door>().SetDoorGlyph("H");
                                        break;
                                    case 0b11:
                                        door.GetComponent<Door>().SetDoorKey(ItemType.KEY_C);
                                        door.GetComponent<Door>().SetDoorGlyph("O");
                                        break;
                                    default:
                                        break;
                                }
                                DoorList.Add(door);
                                door.GetComponent<Door>().SetIsOpened(((remainder >> 3) & 0b1) == 1);
                                door.transform.rotation = Quaternion.Euler(0, dict[directionCounter].Item2 * 90f, 0);
                                door.transform.SetParent(transform);
                                fromTile.AddDoorOnTile(dict[directionCounter], door);
                                toTile.AddDoorOnTile(Tuple.Create(dict[directionCounter].Item1 * -1, dict[directionCounter].Item2 * -1), door);
                            }
                        }
                        directionCounter++;
                        remainder >>= 4;
                    }
                }
            }
        }

        public Tile GetMapTile(Tuple<int, int> pos)
        {
            if (pos.Item1 < 0 || pos.Item1 >= TileObjects.GetLength(0) || pos.Item2 < 0 || pos.Item2 >= TileObjects.GetLength(1))
            {
                //Debug.Log("Out of range");
                return null;
            }
            else
            {
                //Debug.Log("Tile: " + MapManager.Instance.TileObjects[pos[0], pos[1]].name);
                return TileObjects[pos.Item1, pos.Item2].GetComponent<Tile>();
            }
        }
    }
}