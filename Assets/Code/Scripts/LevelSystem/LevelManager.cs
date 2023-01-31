using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Game.MapSystem;
using Unity.Game.ItemSystem;
using Unity.Game.Conditions;
using Unity.Game.RuleSystem;
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
            RuleManager.Instance.InitRule();
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
                //if (Input.GetKeyDown(KeyCode.I))
                //{
                //    (Tile tile, int[] pos) = GetMapTile(GetPos(Player.Instance.Front()));
                //    Debug.Log(tile.name + "at (" + pos[0] + "," + pos[1] + ")");
                //}
                //if (Input.GetKeyDown(KeyCode.J))
                //{
                //    (Tile tile, int[] pos) = GetMapTile(GetPos(Player.Instance.Left()));
                //    Debug.Log(tile.name + "at (" + pos[0] + "," + pos[1] + ")");
                //}
                //if (Input.GetKeyDown(KeyCode.L))
                //{
                //    (Tile tile, int[] pos) = GetMapTile(GetPos(Player.Instance.Right()));
                //    Debug.Log(tile.name + "at (" + pos[0] + "," + pos[1] + ")");
                //}
                //if (Input.GetKeyDown(KeyCode.K))
                //{
                //    (Tile tile, int[] pos) = GetMapTile(GetPos(Player.Instance.Back()));
                //    Debug.Log(tile.name + "at (" + pos[0] + "," + pos[1] + ")");
                //}
                //if (Input.GetKeyDown(KeyCode.M))
                //{
                //    (Tile tile, int[] pos) = GetMapTile(GetPos());
                //    Debug.Log(tile.name + "at (" + pos[0] + "," + pos[1] + ")");
                //}
            }

        }

        public void PlayerMove(Vector3 direction)
        {
            StartCoroutine(OnPlayerMove(direction));
        }

        public IEnumerator OnPlayerMove(Vector3 Direction)
        {
            // check tile behavior
            Tuple<int, int> tilePos = GetPos(Direction);
            Tuple<int, int> playerPos = GetPos();

            Tuple<int, int> movingIntoDirection = Tuple.Create(playerPos.Item1 - tilePos.Item1, playerPos.Item2 - tilePos.Item2);
            Tile moveToTile = MapManager.Instance.GetMapTile(tilePos);
            // if can get tile reference
            if (moveToTile != null)
            {
                // check if tile has a unlockable door
                Door tileDoor = moveToTile.GetDoorOnTile(movingIntoDirection)?.GetComponent<Door>();
                if (tileDoor != null) {
                    yield return tileDoor.TryOpenDoor(ItemList);
                }
                // if can enter, move the player 
                if (moveToTile.IsEnterable(movingIntoDirection) == true)
                {
                    //Debug.Log("Tile is Enterable");
                    //Debug.Log("Player from (" + string.Join(",", GetPos()) + ") Move Into: " + moveToTile.name + " at (" + tilePos[0] + "," + tilePos[1] + ")");
                    yield return Player.Instance.MoveTo(Direction);
                    moveToTile.OnTileEntered();
                }
                else // if cannot, return the player action
                {
                    //Debug.Log("Tile is not Enterable");
                    yield return Player.Instance.OnCannotMoveTo(Direction);
                }
            }
            else // no tile reference, return the player action
            {
                //Debug.Log("Can't Move Into null Tile (" + tilePos.Item1 + "," + tilePos.Item2 + ")");
                yield return Player.Instance.OnCannotMoveTo(Direction);
            }
        }

        public Tuple<int, int> GetPos(Vector3 pos = new Vector3())
        {
            Tuple<int, int> result = Tuple.Create(Mathf.RoundToInt(Player.Instance.posX + pos.x), Mathf.RoundToInt(Player.Instance.posZ + pos.z));
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

        public List<Rule> GetRule()
        {
            List<Rule> result = new List<Rule>();
            foreach (Rule rule in gameMap.MapRules)
            {
                //Debug.Log("Found Rule" + rule.GetDescription());
                result.Add((Rule)rule.Clone());
            }
            return result;
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