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
                OnPlayerMove(Player.Instance.Front());
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                OnPlayerMove(Player.Instance.Left());
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                OnPlayerMove(Player.Instance.Right());
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                OnPlayerMove(Player.Instance.Back());
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

        (Tile, int[]) GetMapTile(int[] pos)
        {
            if (pos[0] < 0 || pos[0] >= MapManager.Instance.TileObjects.GetLength(0) || pos[1] < 0 || pos[1] >= MapManager.Instance.TileObjects.GetLength(1))
            {
                Debug.Log("Out of range");
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

        public void OnPlayerMove(Vector3 Direction)
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
                    Debug.Log("Player from ("+ string.Join(",", GetPos()) + ") Move Into: " + moveToTile.name + " at (" + tilePos[0] + "," + tilePos[1]+")");
                    StartCoroutine(Player.Instance.MoveTo(Direction));
                    moveToTile.OnTileEntered();
                }
                else // if cannot, return the player action
                {
                    Debug.Log("Tile is not Enterable");
                }
            }
            else // no tile reference, return the player action
            {
                Debug.Log("Can't Move Into null Tile (" + tilePos[0] + "," + tilePos[1] + ")");
                return;
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