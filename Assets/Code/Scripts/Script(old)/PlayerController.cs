using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{

    public Vector3 destination;
    public Vector3 rotateDirection;

    public static float speed = 1.5f;
    public float mapScale = 2f;
    private bool isMoving = false;
    private bool isRotating = false;
    private List<string> commandBuffer;
    
    public static bool isPause = false;
    public static bool isReverse = false;
    private List<string> commandStack;

    public static bool isExecuting = false;

    public static int playerX;
    public static int playerZ;

    TextMesh queueText;


    private Animator anim;

    void Start()
    {
        destination = transform.position;
        rotateDirection = transform.forward;
        anim = GetComponent<Animator>();
        commandBuffer = new List<string>();
        commandStack = new List<string>();
    }

    // Update is called once per frame
    void Update()
    {
        MovementController();
        if (!isPause && isExecuting)
        {
            Moving();
            Rotating();
        }


        //animationController
        AnimationUpdate();

        //commandController
        CommandUpdate();

        //debug
        DrawDebugRay();
    }

    void AnimationUpdate()
    {
        if (isReverse)
        {
            anim.speed = -1f;
        }
        if (isPause || !isExecuting)
        {
            anim.speed = 0f;
        }
        else
        {
            anim.speed = 1f;
        }

        if (isMoving)
        {
            anim.SetBool("isMoving", true);
        }
        if (isRotating)
        {
            anim.SetBool("isMoving", false);
        }
        if (!isMoving && commandBuffer.Count == 0)
        {
            anim.SetBool("isMoving", false);
        }
    }

    void SetPlayerMoving(Vector3 direction)
    {
        if (!isMoving)
        {

            destination = transform.position + direction * mapScale;
            if (direction != transform.forward)
            {
                rotateDirection = direction;
                isRotating = true;
            }

            isMoving = true;
        }
    }

    void Moving()
    {
        if (isMoving && !isRotating)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, destination) <= 0.0001f)
            {
                transform.position = Vector3Int.RoundToInt(destination);
                isMoving = false;
            }
        }
    }

    void Rotating()
    {
        if (isRotating)
        {
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, rotateDirection, speed * 4 * Time.deltaTime, 0.0f);
            //Debug.Log(newDirection);

            transform.rotation = Quaternion.LookRotation(newDirection);
            if (Vector3.Distance(transform.forward, rotateDirection) <= 0.01f)
            {
                transform.rotation = Quaternion.LookRotation(rotateDirection);
                isRotating = false;
            }
        }

    }

    void DrawDebugRay()
    {
        Debug.DrawRay(transform.position, transform.forward, Color.red);
        Debug.DrawRay(transform.position, Quaternion.Euler(0, -90, 0) * transform.forward, Color.blue);
        Debug.DrawRay(transform.position, Quaternion.Euler(0, 90, 0) * transform.forward, Color.blue);
    }

    void MovementController()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isPause = !isPause;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            isReverse = !isReverse;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isExecuting = !isExecuting;
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            switch (speed)
            {
                case 2f:
                    speed = 4f;
                    break;
                case 4f:
                    speed = 2f;
                    break;
                default:
                    break;
            }
        }

        if (commandBuffer.Count <= 4)
        {
            // Enqueue Command
            if (Input.GetKeyDown(KeyCode.W))
            {
                commandBuffer.Add("forward");
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                commandBuffer.Add("left");
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                commandBuffer.Add("right");
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                commandBuffer.Add("back");
            }
        }

        if (commandBuffer.Count > 0)
        {
            // Dequeue Command
            if (!isMoving && !isRotating && isExecuting)
            {
                switch (commandBuffer[0])
                {
                    case "forward":
                        SetPlayerMoving(transform.forward);
                        break;
                    case "left":
                        SetPlayerMoving(Quaternion.Euler(0, -90, 0) * transform.forward);
                        break;
                    case "right":
                        SetPlayerMoving(Quaternion.Euler(0, 90, 0) * transform.forward);
                        break;
                    case "back":
                        SetPlayerMoving(Quaternion.Euler(0, 180, 0) * transform.forward);
                        break;
                    default:
                        break;
                }
                commandStack.Insert(0, commandBuffer[0]);
                commandBuffer.RemoveAt(0);
            }

        }


    }

    void CommandUpdate()
    {
        GameObject.Find("UsedCommand").GetComponent<TMP_Text>().text = string.Join('\n', commandStack.ToArray());
        GameObject.Find("QueueCommand").GetComponent<TMP_Text>().text = string.Join('\n', commandBuffer.ToArray());
    }
}
