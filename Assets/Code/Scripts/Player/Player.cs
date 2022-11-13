using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalConfig;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    [SerializeField] private bool isMoving = false;
    public int[] FrontPos()
    {
        Vector3 pos = transform.forward;
        return new int[] { posX + (int)pos.x, posZ + (int)pos.z };
    }
    public int[] LeftPos()
    {
        Vector3 pos = Quaternion.Euler(0, -90, 0) * transform.forward;
        return new int[] { posX + (int)pos.x, posZ + (int)pos.z };
    }
    public int[] RightPos()
    {
        Vector3 pos = Quaternion.Euler(0, 90, 0) * transform.forward;
        return new int[] { posX + (int)pos.x, posZ + (int)pos.z };
    }
    public int[] BackPos()
    {
        Vector3 pos = Quaternion.Euler(0, 180, 0) * transform.forward;
        return new int[] { posX + (int)pos.x, posZ + (int)pos.z };
    }

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

    public IEnumerator MoveTo(Vector3 destination)
    {
        if (isMoving)
        {
            yield break;
        }
        isMoving = true;
        //rotate first
        Debug.Log("rotate distance" + Vector3.Distance(transform.forward, destination) + transform.forward + "" + destination);
        //while (Vector3.Distance(transform.forward, destination) >= 0.0001f)
        //{
        //    //Vector3.RotateTowards(transform.forward, rotateDirection, speed * 4 * Time.deltaTime, 0.0f);
        //    transform.forward = Vector3.RotateTowards(transform.forward, destination, 4 * Time.deltaTime, 0.0f);
        //    //transform.forward = Vector3.MoveTowards(transform.forward, destination, PlayerConfig.PLAYER_ROTATE_SPEED * Time.deltaTime);
        //    yield return null;
        //}
        //transform.forward = Vector3Int.RoundToInt(destination);

        //then move
        while (Vector3.Distance(transform.position, destination) >= 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, PlayerConfig.PLAYER_MOVE_SPEED * Time.deltaTime);
            yield return null;
        }
        transform.position = Vector3Int.RoundToInt(destination);
        isMoving = false;
    }

    public IEnumerator Move(int[] pos)
    {
        Debug.Log("test move " + pos[0] + " : " + pos[1]);
        yield return MoveTo(new Vector3(pos[0] * MapConfig.TILE_SCALE, posY, pos[1] * MapConfig.TILE_SCALE));
        Debug.Log("test move ended");
    }



    // Update is called once per frame
    void Update()
    {
        DrawDebugRay();
    }

    void DrawDebugRay()
    {
        Debug.DrawRay(transform.position, transform.forward * MapConfig.TILE_SCALE / 2, Color.red);
        Debug.DrawRay(transform.position, Quaternion.Euler(0, -90, 0) * transform.forward * MapConfig.TILE_SCALE / 2, Color.blue);
        Debug.DrawRay(transform.position, Quaternion.Euler(0, 90, 0) * transform.forward * MapConfig.TILE_SCALE / 2, Color.blue);
    }
}
