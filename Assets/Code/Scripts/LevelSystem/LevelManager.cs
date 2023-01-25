using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Game.MapSystem;
using Unity.Game.ItemSystem;
using Unity.Game.Conditions;
using GlobalConfig;

namespace Unity.Game.Level
{

    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }


        // level stats
        public Map gameMap;

        // player stats
        public List<ItemType> ItemList;
        public ConditionSign lastSign = ConditionSign.EMPTY;

        // Start is called before the first frame update
        void Awake()
        {
            Debug.Log("LevelManager Awake");
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
            Map map = new Map();
            InitLevel(map);
        }

        void InitLevel(Map map)
        {
            ItemList = new List<ItemType>();
            lastSign = ConditionSign.EMPTY;
            SetMap(map);
            MapManager.Instance.InitMap();
            ItemManager.Instance.InitItems();
            ConditionPickerController.Instance.InitConditionPicker();
            InitPlayer();
        }

        void InitPlayer()
        {
            (int[] playerPosition, int[] playerRotation) = gameMap.GetPlayerInit();
            SetPlayerPosition(playerPosition[0], playerPosition[1]);
            SetPlayerRotation(playerRotation[0], playerRotation[1]);
        }

        // Update is called once per frame
        void Update()
        {

            // movement and map debug
            if (GlobalConfig.LevelConfig.LEVEL_DEBUG_MODE == true)
            {
                // reset movement
                if (Input.GetKeyDown(KeyCode.R))
                {
                    SetPlayerPosition(3, 0);
                    SetPlayerRotation(1, 1);
                }

                // movement test 
                if (Input.GetKeyDown(KeyCode.W))
                {
                    PlayerMove(Player.Instance.Front());
                }
                if (Input.GetKeyDown(KeyCode.A))
                {
                    PlayerMove(Player.Instance.Left());
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    PlayerMove(Player.Instance.Right());
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    PlayerMove(Player.Instance.Back());
                }

                // tile from direction test
                if (Input.GetKeyDown(KeyCode.I))
                {
                    (Tile tile, int[] pos) = GetMapTile(GetPos(Player.Instance.Front()));
                    Debug.Log(tile.name + "at (" + pos[0] + "," + pos[1] + ")");
                }
                if (Input.GetKeyDown(KeyCode.J))
                {
                    (Tile tile, int[] pos) = GetMapTile(GetPos(Player.Instance.Left()));
                    Debug.Log(tile.name + "at (" + pos[0] + "," + pos[1] + ")");
                }
                if (Input.GetKeyDown(KeyCode.L))
                {
                    (Tile tile, int[] pos) = GetMapTile(GetPos(Player.Instance.Right()));
                    Debug.Log(tile.name + "at (" + pos[0] + "," + pos[1] + ")");
                }
                if (Input.GetKeyDown(KeyCode.K))
                {
                    (Tile tile, int[] pos) = GetMapTile(GetPos(Player.Instance.Back()));
                    Debug.Log(tile.name + "at (" + pos[0] + "," + pos[1] + ")");
                }
                if (Input.GetKeyDown(KeyCode.M))
                {
                    (Tile tile, int[] pos) = GetMapTile(GetPos());
                    Debug.Log(tile.name + "at (" + pos[0] + "," + pos[1] + ")");
                }
            }

        }

        (Tile, int[]) GetMapTile(int[] pos)
        {
            if (pos[0] < 0 || pos[0] >= MapManager.Instance.TileObjects.GetLength(0) || pos[1] < 0 || pos[1] >= MapManager.Instance.TileObjects.GetLength(1))
            {
                //Debug.Log("Out of range");
                return (null, pos);
            }
            else
            {
                //Debug.Log("Tile: " + MapManager.Instance.TileObjects[pos[0], pos[1]].name);
                return (MapManager.Instance.TileObjects[pos[0], pos[1]].GetComponent<Tile>(), pos);
            }
        }
        (Tile, int[]) GetMapTile(Vector3 direction)
        {
            int[] pos = GetPos(direction);
            return GetMapTile(pos);
        }

        public void PlayerMove(Vector3 direction)
        {
            StartCoroutine(OnPlayerMove(direction));
        }

        public IEnumerator OnPlayerMove(Vector3 Direction)
        {
            // check tile behavior
            (Tile moveToTile, int[] tilePos) = GetMapTile(Direction);
            // if can get tile reference
            if (moveToTile != null)
            {
                // if can enter, move the player 
                if (moveToTile.IsEnterable() == true)
                {
                    Debug.Log("Tile is Enterable");
                    Debug.Log("Player from (" + string.Join(",", GetPos()) + ") Move Into: " + moveToTile.name + " at (" + tilePos[0] + "," + tilePos[1] + ")");
                    yield return Player.Instance.MoveTo(Direction);
                    moveToTile.OnTileEntered();
                }
                else // if cannot, return the player action
                {
                    Debug.Log("Tile is not Enterable");
                    yield return Player.Instance.OnCannotMoveTo(Direction);
                }
            }
            else // no tile reference, return the player action
            {
                Debug.Log("Can't Move Into null Tile (" + tilePos[0] + "," + tilePos[1] + ")");
                yield return Player.Instance.OnCannotMoveTo(Direction);
            }
        }

        public int[] GetPos(Vector3 pos = new Vector3())
        {
            int[] result = new int[] { Mathf.RoundToInt(Player.Instance.posX + pos.x), Mathf.RoundToInt(Player.Instance.posZ + pos.z) };
            //Debug.Log("PosX: "+ Player.Instance.posX +"+"+ Mathf.RoundToInt(pos.x) + "= "+ Mathf.RoundToInt(Player.Instance.posX + pos.x) + "\nposZ: " + Player.Instance.posZ + "+" + Mathf.RoundToInt(pos.z) + "= " + Mathf.RoundToInt(Player.Instance.posZ + pos.z));
            return result;
        }

        void SetPlayerPosition(int x, int z)
        {
            Player.Instance.transform.position = new Vector3(x * MapConfig.TILE_SCALE, Player.Instance.transform.position.y, z * MapConfig.TILE_SCALE);
        }
        void SetPlayerRotation(int x, int z)
        {
            if (x == 0 && z == 0)
            {
                Player.Instance.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (x == 0 && z == 1)
            {
                Player.Instance.transform.rotation = Quaternion.Euler(0, 90, 0);
            }
            else if (x == 1 && z == 0)
            {
                Player.Instance.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (x == 1 && z == 1)
            {
                Player.Instance.transform.rotation = Quaternion.Euler(0, 270, 0);
            }
        }

        public Map GetMap()
        {
            return gameMap;
        }

        public void SetMap(Map map)
        {
            gameMap = map;
        }


        public void SetLastSign(ConditionSign sign)
        {
            lastSign = sign;
        }
        public ConditionSign GetLastSign()
        {
            return lastSign;
        }

        public void AddItem(ItemType item)
        {
            ItemList.Add(item);
        }

        public void RemoveItem(ItemType item)
        {
            ItemList.Remove(item);

        }

    }
}