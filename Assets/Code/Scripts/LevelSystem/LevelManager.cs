using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Game.Map;
using GlobalConfig;

namespace Unity.Game.Level
{

    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }
        public int[] playerPosition = new int[] { 0, 0 };

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

        // Update is called once per frame
        void Update()
        {
            // reset movement
            if (Input.GetKeyDown(KeyCode.R))
            {
                SetPlayerPosition(0, 0);
                SetPlayerRotation(1, 0);
            }

            // movement test
            if (Input.GetKeyDown(KeyCode.W))
            {
                Debug.Log("player move from " + string.Join(",", GetPos()) + " to " + string.Join(",", GetPos(Player.Instance.Front())));
                StartCoroutine(Player.Instance.MoveTo(Player.Instance.Front()));
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                Debug.Log("player move from " + string.Join(",", GetPos()) + " to " + string.Join(",", GetPos(Player.Instance.Left())));
                StartCoroutine(Player.Instance.MoveTo(Player.Instance.Left()));
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                Debug.Log("player move from " + string.Join(",", GetPos()) + " to " + string.Join(",", GetPos(Player.Instance.Right())));
                StartCoroutine(Player.Instance.MoveTo(Player.Instance.Right()));
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                Debug.Log("player move from " + string.Join(",", GetPos()) + " to " + string.Join(",", GetPos(Player.Instance.Back())));
                StartCoroutine(Player.Instance.MoveTo(Player.Instance.Back()));
            }

            // tile from direction test
            if (Input.GetKeyDown(KeyCode.I))
            {
                GetMapTile(GetPos(Player.Instance.Front()));
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                GetMapTile(GetPos(Player.Instance.Left()));
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                GetMapTile(GetPos(Player.Instance.Right()));
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                GetMapTile(GetPos(Player.Instance.Back()));
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                GetMapTile(GetPos());
            }
        }

        Tile GetMapTile(int[] pos)
        {
            if (pos[0] < 0 || pos[0] >= MapManager.Instance.TileObjects.GetLength(0) || pos[1] < 0 || pos[1] >= MapManager.Instance.TileObjects.GetLength(1))
            {
                Debug.Log("Out of range");
                return null;
            }
            else
            {
                Debug.Log(MapManager.Instance.TileObjects[pos[0], pos[1]].name);
                return MapManager.Instance.TileObjects[pos[0], pos[1]].GetComponent<Tile>();
            }
        }

        public int[] GetPos(Vector3 pos = new Vector3())
        {
            Debug.Log(pos);
            return new int[] { Player.Instance.posX + (int)pos.x, Player.Instance.posZ + (int)pos.z };
        }

        void SetPlayerPosition(int x, int z)
        {
            playerPosition[0] = x;
            playerPosition[1] = z;
            Player.Instance.transform.position = new Vector3(playerPosition[0] * MapConfig.TILE_SCALE, Player.Instance.transform.position.y, playerPosition[1] * MapConfig.TILE_SCALE);
        }

        void SetPlayerRotation(int x, int z)
        {
            if (x == 0 && z == 1)
            {
                Player.Instance.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (x == 1 && z == 0)
            {
                Player.Instance.transform.rotation = Quaternion.Euler(0, 90, 0);
            }
            else if (x == 0 && z == -1)
            {
                Player.Instance.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (x == -1 && z == 0)
            {
                Player.Instance.transform.rotation = Quaternion.Euler(0, 270, 0);
            }
        }
    }
}