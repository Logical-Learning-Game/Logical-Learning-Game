using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalConfig;
using Unity.Game.Conditions;
using Unity.Game.ItemSystem;
using Unity.Game.Command;

namespace Unity.Game
{

    public class Player : MonoBehaviour
    {
        public static Player Instance { get; private set; }
        [SerializeField] private bool isMoving = false;
        [SerializeField] private bool isBored = false;
        [SerializeField] private float TimeUntilBored = 5f;
        [SerializeField] private float ElapsedTime;

        public Animator CharacterAnimator;
        public Vector3 Front()
        {
            return transform.forward;
        }
        public Vector3 Left()
        {
            return Quaternion.Euler(0, -90, 0) * transform.forward;
        }
        public Vector3 Right()
        {
            return Quaternion.Euler(0, 90, 0) * transform.forward;
        }
        public Vector3 Back()
        {
            return Quaternion.Euler(0, 180, 0) * transform.forward;
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
                CharacterAnimator = GetComponentInChildren<Animator>();
            }
            else
            {
                Destroy(gameObject);
            }
        }


        public IEnumerator MoveTo(Vector3 direction)
        {
            if (CommandManager.Instance.isExecuting == false)
            {
                isMoving = false;
                yield break;
            }

            if (isMoving)
            {
                yield break;
            }
            Vector3 destination = transform.position + new Vector3(direction.x * MapConfig.TILE_SCALE, 0, direction.z * MapConfig.TILE_SCALE);
            isMoving = true;

            //rotate first
            SetEndPlayerMove();
            AudioManager.PlayCharacterStepSound();
            while (Vector3.Distance(transform.forward, direction) >= 0.01f)
            {
                if(CommandManager.Instance.isExecuting == false)
                {
                    isMoving = false;
                    yield break;
                }
                Vector3 splitRotation = Vector3.RotateTowards(transform.forward, direction, PlayerConfig.PLAYER_ROTATE_SPEED * Time.deltaTime, 0.0f);
                transform.rotation = Quaternion.LookRotation(splitRotation);
                yield return null;
            }
            transform.rotation = Quaternion.LookRotation(direction);

            //then move 
            SetPlayerMove();
            while (Vector3.Distance(transform.position, destination) >= 0.01f)
            {
                if (CommandManager.Instance.isExecuting == false)
                {
                    isMoving = false;
                    yield break;
                }
                transform.position = Vector3.MoveTowards(transform.position, destination, PlayerConfig.PLAYER_MOVE_SPEED * Time.deltaTime);
                yield return null;
            }
            transform.position = Vector3Int.RoundToInt(destination);
            isMoving = false;
            SetEndPlayerMove();
        }

        public IEnumerator OnCannotMoveTo(Vector3 direction)
        {
            if (CommandManager.Instance.isExecuting == false)
            {
                isMoving = false;
                yield break;
            }

            if (isMoving)
            {
                yield break;
            }
            // save current position and direction
            Vector3 origin = transform.position;
            Vector3 originDirection = transform.forward;
            // get reversed direction
            Vector3 reverseDirection = Quaternion.Euler(0, 180, 0) * direction;
            // destination is only 2/5 of waypoint
            Vector3 destination = transform.position + new Vector3(direction.x * MapConfig.TILE_SCALE * MapConfig.TILE_INVESTIGATE_DISTANCE, 0, direction.z * MapConfig.TILE_SCALE * MapConfig.TILE_INVESTIGATE_DISTANCE);
            isMoving = true;

            //rotate first
            AudioManager.PlayCharacterStepSound();
            SetEndPlayerMove();
            while (Vector3.Distance(transform.forward, direction) >= 0.01f)
            {
                if (CommandManager.Instance.isExecuting == false)
                {
                    isMoving = false;
                    yield break;
                }
                Vector3 splitRotation = Vector3.RotateTowards(transform.forward, direction, PlayerConfig.PLAYER_ROTATE_SPEED * Time.deltaTime, 0.0f);
                transform.rotation = Quaternion.LookRotation(splitRotation);
                yield return null;
            }
            transform.rotation = Quaternion.LookRotation(direction);

            //then move 
            SetPlayerMove();
            while (Vector3.Distance(transform.position, destination) >= 0.01f)
            {
                if (CommandManager.Instance.isExecuting == false)
                {
                    isMoving = false;
                    yield break;
                }
                transform.position = Vector3.MoveTowards(transform.position, destination, PlayerConfig.PLAYER_MOVE_SPEED * Time.deltaTime);
                yield return null;
            }
            transform.position = Vector3Int.RoundToInt(destination);
            AudioManager.PlayDefaultWarningSound();
            SetEndPlayerMove();
            yield return new WaitForSeconds(PlayerConfig.PLAYER_INVESTIGATE_TIME);

            //rotate back
            while (Vector3.Distance(transform.forward, reverseDirection) >= 0.01f)
            {
                if (CommandManager.Instance.isExecuting == false)
                {
                    isMoving = false;
                    yield break;
                }
                Vector3 splitRotation = Vector3.RotateTowards(transform.forward, reverseDirection, PlayerConfig.PLAYER_ROTATE_SPEED * Time.deltaTime, 0.0f);
                transform.rotation = Quaternion.LookRotation(splitRotation);
                yield return null;
            }
            transform.rotation = Quaternion.LookRotation(reverseDirection);

            //move back
            SetPlayerMove();
            while (Vector3.Distance(transform.position, origin) >= 0.01f)
            {
                if (CommandManager.Instance.isExecuting == false)
                {
                    isMoving = false;
                    yield break;
                }
                transform.position = Vector3.MoveTowards(transform.position, origin, PlayerConfig.PLAYER_MOVE_SPEED * Time.deltaTime);
                yield return null;
            }
            transform.position = Vector3Int.RoundToInt(origin);

            //rotate to origin direction
            SetEndPlayerMove();
            while (Vector3.Distance(transform.forward, originDirection) >= 0.01f)
            {
                if (CommandManager.Instance.isExecuting == false)
                {
                    isMoving = false;
                    yield break;
                }
                Vector3 splitRotation = Vector3.RotateTowards(transform.forward, originDirection, PlayerConfig.PLAYER_ROTATE_SPEED * Time.deltaTime, 0.0f);
                transform.rotation = Quaternion.LookRotation(splitRotation);
                yield return null;
            }
            transform.rotation = Quaternion.LookRotation(originDirection);

            isMoving = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (!isMoving)
            {
                if (!isBored)
                {
                    ElapsedTime += Time.deltaTime;
                    if (ElapsedTime > TimeUntilBored)
                    {
                        isBored = true;
                        SetPlayerBoredAnimation();
                        ElapsedTime = 0;

                    }
                }
                else if (isBored)
                {
                    ElapsedTime += Time.deltaTime;
                    if (ElapsedTime > TimeUntilBored)
                    {
                        isBored = false;
                        SetPlayerIdle();
                        ElapsedTime = 0;
                    }
                }
            }
            else
            {
                isBored = false;
                ElapsedTime = 0;
            }

        }

        public void SetPlayerMove()
        {
            CharacterAnimator.SetBool("Run", true);
            CharacterAnimator.SetBool("Eat", false);
            CharacterAnimator.SetBool("Turn Head", false);
        }

        public void SetEndPlayerMove()
        {
            CharacterAnimator.SetBool("Run", false);
        }

        public void SetPlayerIdle()
        {
            CharacterAnimator.SetBool("Run", false);
            CharacterAnimator.SetBool("Eat", false);
            CharacterAnimator.SetBool("Turn Head", false);
            TimeUntilBored = Random.Range(10, 20);
        }

        public void SetPlayerBoredAnimation()
        {
            int rand = Random.Range(0, 2);
            switch (rand)
            {
                case 0:
                    CharacterAnimator.SetBool("Eat", true);
                    CharacterAnimator.SetBool("Turn Head", false);
                    TimeUntilBored = Random.Range(2, 6);
                    break;
                case 1:
                    CharacterAnimator.SetBool("Eat", false);
                    CharacterAnimator.SetBool("Turn Head", true);
                    TimeUntilBored = Random.Range(3, 10);
                    break;
                default: break;
            }
        }
    }
}