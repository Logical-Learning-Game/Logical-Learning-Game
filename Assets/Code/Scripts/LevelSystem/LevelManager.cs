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
        public int[] playerPosition = new int[] {0,0};
        
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
            if (Input.GetKeyDown(KeyCode.W))
            {
                SetPlayerPosition(3, 3);
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                Debug.Log(Player.Instance.posX+" : "+ Player.Instance.posZ);
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                StartCoroutine(Player.Instance.MoveBy(1, 0));
            }
        }

        void SetPlayerPosition(int x,int z)
        {
            playerPosition[0] = x;
            playerPosition[1] = z;
            Player.Instance.transform.position = new Vector3(playerPosition[0] * MapConfig.TILE_SCALE, Player.Instance.transform.position.y, playerPosition[1] * MapConfig.TILE_SCALE);
        }
    }
}