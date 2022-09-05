using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridManager : MonoBehaviour
{
    public float mapScale = 2f;
    public GameObject tilePath;
    public GameObject goal;
    public GameObject tileBlocked;
    public GameObject player;
    public Vector3 leftBottomLocation = new(0, 0, 0);

    // . => empty tile
    // p => player start position
    // g => goal tile
    // x => block tile / blank space
    // * => score tile / key drop

    public string[,] mapArray = new string[,] {
        { ".", "p", ".", "." ,"." },
        { ".", "x", ".", "x" ,"."},
        { ".", ".", ".", "." ,"."},
        { ".", "x", ".", "x" ,"."},
        { ".", ".", ".", "." ,"g"}
    };

    private void Awake()
    {
        if (tilePath && goal && tileBlocked)
        {
            generateGrid();
        }
    }

    private void generateGrid()
    {
        for (int x = 0; x < mapArray.GetLength(0); x++)
        {
            for (int z = 0; z < mapArray.GetLength(1); z++)
            {
                if (mapArray[x, z] == ".")
                {
                    GameObject tile = Instantiate(tilePath, new Vector3(leftBottomLocation.x + mapScale * x, leftBottomLocation.y, leftBottomLocation.z + mapScale * z), Quaternion.identity);
                    tile.name = SetTileName(x, z);
                    tile.transform.localScale = new Vector3(mapScale, 1f, mapScale);
                    tile.transform.SetParent(gameObject.transform);
                    //Instantiate(tile, new Vector3(x, 0, z), Quaternion.identity);
                }
                else if (mapArray[x, z] == "p")
                {
                    GameObject tile = Instantiate(tilePath, new Vector3(leftBottomLocation.x + mapScale * x, leftBottomLocation.y, leftBottomLocation.z + mapScale * z), Quaternion.identity);
                    tile.name = SetTileName(x, z);
                    tile.transform.localScale = new Vector3(mapScale, 1f, mapScale);
                    tile.transform.SetParent(gameObject.transform);


                    Instantiate(player, new Vector3(leftBottomLocation.x + mapScale * x, leftBottomLocation.y, leftBottomLocation.z + mapScale * z), Quaternion.identity);
                    PlayerController.playerX = x;
                    PlayerController.playerZ = z;
                }
                else if (mapArray[x, z] == "g")
                {
                    
                    GameObject tile = Instantiate(tilePath, new Vector3(leftBottomLocation.x + mapScale * x, leftBottomLocation.y, leftBottomLocation.z + mapScale * z), Quaternion.identity);
                    tile.name = SetTileName(x, z);
                    tile.transform.localScale = new Vector3(mapScale, 1f, mapScale);
                    tile.transform.SetParent(gameObject.transform);
                   
                    GameObject goalItem = Instantiate(goal, new Vector3(leftBottomLocation.x + mapScale * x, leftBottomLocation.y, leftBottomLocation.z + mapScale * z), Quaternion.identity);
                    goalItem.transform.localScale = new Vector3(mapScale, 1f, mapScale);
                    GameObject fx = goalItem.GetComponentInChildren<ParticleSystem>().gameObject;
                    fx.transform.localScale = new Vector3(mapScale, 1f, mapScale);
                    goalItem.transform.SetParent(tile.transform);

                }
                else if (mapArray[x, z] == "x")
                {
                    GameObject tile = Instantiate(tileBlocked, new Vector3(leftBottomLocation.x + mapScale * x, leftBottomLocation.y, leftBottomLocation.z + mapScale * z), Quaternion.identity);
                    tile.name = SetTileName(x, z);
                    tile.transform.localScale = new Vector3(mapScale, 1f, mapScale);
                    tile.transform.SetParent(gameObject.transform);
                }
                //GameObject tile = Instantiate(gridPrefab[mapArray[x,z]], new Vector3(leftBottomLocation.x + mapScale*x, leftBottomLocation.y, leftBottomLocation.z + mapScale*z), Quaternion.identity);
                //tile.transform.localScale = new Vector3(mapScale, 1f, mapScale);
                //tile.transform.SetParent(gameObject.transform);
            }
        }
    }

    private string SetTileName(int x, int z) => "tile_" + x + "_" + z;
  


}
