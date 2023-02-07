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
        public static event Action GameWon;

        // level stats
        public Map gameMap;
        public bool isPlayerReachGoal = false;

        // player stats
        public List<ItemType> ItemList;
        public ConditionSign lastSign = ConditionSign.EMPTY;
        
        [SerializeField] private GameObject LevelIndicator;

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
            isPlayerReachGoal = false;
            SetMap(map);
            LevelIndicator.GetComponent<TMPro.TMP_Text>().text = map.MapName;
            MapManager.Instance.InitMap();
            ItemManager.Instance.InitItems();
            ConditionPickerController.Instance.InitConditionPicker();
            RuleManager.Instance.InitRule();
            InitPlayer();
        }

        void InitPlayer()
        {
            (int[] playerPosition, int[] playerRotation) = GetPlayerInitValue(gameMap);
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
                    InitLevel(gameMap);
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
                    yield return Player.Instance.MoveTo(Direction);
                    moveToTile.OnTileEntered();
                    RuleManager.Instance.OnPlayCheck();
                    if (moveToTile is GoalTile)
                    {
                        RuleManager.Instance.OnClearCheck();
                    }
                   
                }
                else // if cannot, return the player action
                {
                    yield return Player.Instance.OnCannotMoveTo(Direction);
                }
            }
            else // no tile reference, return the player action
            {
                yield return Player.Instance.OnCannotMoveTo(Direction);
            }
        }

        public Tuple<int, int> GetPos(Vector3 pos = new Vector3())
        {
            Tuple<int, int> result = Tuple.Create(Mathf.RoundToInt(Player.Instance.posX + pos.x), Mathf.RoundToInt(Player.Instance.posZ + pos.z));
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
                result.Add(rule);
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

        public void SetIsPlayerReachGoal()
        {
            isPlayerReachGoal = true;
            GameWon?.Invoke();
        }

        public bool GetIsPlayerReachGoal()
        {
            return isPlayerReachGoal;
        }

        public (int[], int[]) GetPlayerInitValue(Map map)
        {
            uint[,] MapData = map.MapData;
            int[] playerPosition = new int[2] { 0, 0 };
            int[] playerRotation = new int[2] { 0, 0 };
            for (int i = 0; i < MapData.GetLength(0); i++)
            {
                for (int j = 0; j < MapData.GetLength(1); j++)
                {
                    if ((MapData[i, j] & 0b1) == 1)
                    {
                        playerPosition = new int[2] { i, j };
                        playerRotation = new int[2] { (int)(MapData[i, j] >> 2) & 0b1, (int)(MapData[i, j] >> 1) & 0b1 };

                        return (playerPosition, playerRotation);
                    }
                }
            }
            return (playerPosition, playerRotation);
        }

        public HashSet<ConditionSign> GetUniqueConditions()
        {
            uint[,] MapData = gameMap.MapData;
            HashSet<ConditionSign> uniqueConditions = new HashSet<ConditionSign>();
            foreach (uint data in MapData)
            {
                uint tile = data >> 4 & 0b1111;
                if (tile == 0b0011)
                {
                    uniqueConditions.Add(ConditionSign.A);
                }
                else if (tile == 0b0100)
                {
                    uniqueConditions.Add(ConditionSign.B);
                }
                else if (tile == 0b0101)
                {
                    uniqueConditions.Add(ConditionSign.C);
                }
                else if (tile == 0b0110)
                {
                    uniqueConditions.Add(ConditionSign.D);
                }
                else if (tile == 0b0111)
                {
                    uniqueConditions.Add(ConditionSign.E);
                }
            }
            return uniqueConditions;
        }

    }
}