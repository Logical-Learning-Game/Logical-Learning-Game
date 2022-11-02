using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        private float tileScale = 6f;
        public static MapManager Instance { get; private set; }
        // temporary map string
        // E = Empty
        // O = Obstracle
        // C = Condition
        // G = Goal
        // D = Door

        private string[,] textTiles = new string[,]
        {
           { "E","O","O","G" },
           { "E","O","O","D" },
           { "E","O","O","D" },
           { "E","E","E","E" }
        };

        public GameObject[,] TileObjects;
        // Start is called before the first frame update
        void Awake()
        {
            Debug.Log("MapManager Awake");
            if (Instance == null)
            {
                Debug.Log("MapManager Instance is null");
                Instance = this;
                DestroyMap();
                CreateMap();
            }
            else
            {
                Debug.Log("MapManager Instance is not null ");
                Destroy(gameObject);
            }

        }

        // Update is called once per frame
        void Update()
        {

        }

        void CreateMap()
        {
            TileObjects = new GameObject[textTiles.GetLength(0), textTiles.GetLength(1)];
            for (int i = 0; i < textTiles.GetLength(0); i++)
            {
                for (int j = 0; j < textTiles.GetLength(1); j++)
                {
                    switch (textTiles[i, j])
                    {
                        case "E":
                            TileObjects[i, j] = Instantiate(EmptyTile, new Vector3(i * tileScale, 0, j * tileScale), Quaternion.identity);
                            TileObjects[i, j].transform.SetParent(transform);
                            break;
                        case "O":
                            TileObjects[i, j] = Instantiate(ObstracleTile, new Vector3(i * tileScale, 0, j * tileScale), Quaternion.identity);
                            TileObjects[i, j].transform.SetParent(transform);
                            break;
                        case "G":
                            TileObjects[i, j] = Instantiate(GoalTile, new Vector3(i * tileScale, 0, j * tileScale), Quaternion.identity);
                            TileObjects[i, j].transform.SetParent(transform);
                            break;
                        case "D":
                            TileObjects[i, j] = Instantiate(DoorTile, new Vector3(i * tileScale, 0, j * tileScale), Quaternion.identity);
                            TileObjects[i, j].transform.SetParent(transform);
                            break;
                        case "C":
                            TileObjects[i, j] = Instantiate(ConditionTile, new Vector3(i * tileScale, 0, j * tileScale), Quaternion.identity);
                            TileObjects[i, j].transform.SetParent(transform);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        void DestroyMap()
        {
            for (int i = this.transform.childCount; i > 0; --i)
                DestroyImmediate(this.transform.GetChild(0).gameObject);
        }
    }
}