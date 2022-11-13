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
                //StartCoroutine(Player.Instance.MoveBy(1, 0));
                StartCoroutine(Player.Instance.Move(Player.Instance.FrontPos()));
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                //StartCoroutine(Player.Instance.MoveBy(0, 1));
                StartCoroutine(Player.Instance.Move(Player.Instance.LeftPos()));
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                //StartCoroutine(Player.Instance.MoveBy(0, -1));
                StartCoroutine(Player.Instance.Move(Player.Instance.RightPos()));
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                //StartCoroutine(Player.Instance.MoveBy(-1, 0));
                StartCoroutine(Player.Instance.Move(Player.Instance.BackPos()));
            }


            // tile from direction test
            if (Input.GetKeyDown(KeyCode.I))
            {
                GetMapTile(Player.Instance.FrontPos());
                //Debug.Log(Player.Instance.FrontPos()[0] + " : " + Player.Instance.FrontPos()[1]);
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                GetMapTile(Player.Instance.LeftPos());
                //Debug.Log(Player.Instance.LeftPos()[0] + " : " + Player.Instance.LeftPos()[1]);
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                GetMapTile(Player.Instance.RightPos());

                //Debug.Log(Player.Instance.RightPos()[0] + " : " + Player.Instance.RightPos()[1]);
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                GetMapTile(Player.Instance.BackPos());
                //Debug.Log(Player.Instance.BackPos()[0] + " : " + Player.Instance.BackPos()[1]);
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