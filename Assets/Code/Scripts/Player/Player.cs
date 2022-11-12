using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalConfig;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public int posX
    {
        get
        {
            return (int)(transform.position.x / MapConfig.TILE_SCALE);
        }
    }

    public int posY
    {
        get
        {
            return (int)(transform.position.y);
        }
    }
    
    public int posZ
    {
        get
        {
            return (int)(transform.position.z / MapConfig.TILE_SCALE);
        }
    }
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
    public IEnumerator MoveBy(int x, int z)
    {
        Debug.Log("test move by " + x + " : " + z);
        yield return MoveTo(new Vector3((posX+x) * MapConfig.TILE_SCALE, posY, (posZ+z) * MapConfig.TILE_SCALE));
        Debug.Log("test move ended");
    }

    public IEnumerator MoveTo(Vector3 destination)
    {
        while (Vector3.Distance(transform.position, destination) >= 0.0001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, PlayerConfig.PLAYER_MOVE_SPEED * Time.deltaTime);
            yield return null;
        }
        transform.position = Vector3Int.RoundToInt(destination);
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
