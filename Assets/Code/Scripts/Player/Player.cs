using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalConfig;

namespace Unity.Game
{

    public class Player : MonoBehaviour
    {
        public static Player Instance { get; private set; }
        [SerializeField] private bool isMoving = false;

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
            }
            else
            {
                Destroy(gameObject);
            }
        }
        public IEnumerator MoveTo(Vector3 direction)
        {
            if (isMoving)
            {
                yield break;
            }
            Vector3 destination = transform.position + new Vector3(direction.x * MapConfig.TILE_SCALE, 0, direction.z * MapConfig.TILE_SCALE);
            isMoving = true;

            //rotate first
            while (Vector3.Distance(transform.forward, direction) >= 0.01f)
            {
                Vector3 splitRotation = Vector3.RotateTowards(transform.forward, direction, PlayerConfig.PLAYER_ROTATE_SPEED * Time.deltaTime, 0.0f);
                transform.rotation = Quaternion.LookRotation(splitRotation);
                yield return null;
            }
            transform.rotation = Quaternion.LookRotation(direction);

            //then move 
            while (Vector3.Distance(transform.position, destination) >= 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, destination, PlayerConfig.PLAYER_MOVE_SPEED * Time.deltaTime);
                yield return null;
            }
            transform.position = Vector3Int.RoundToInt(destination);
            isMoving = false;
        }

        public IEnumerator OnCannotMoveTo(Vector3 direction)
        {
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
            while (Vector3.Distance(transform.forward, direction) >= 0.01f)
            {
                Vector3 splitRotation = Vector3.RotateTowards(transform.forward, direction, PlayerConfig.PLAYER_ROTATE_SPEED * Time.deltaTime, 0.0f);
                transform.rotation = Quaternion.LookRotation(splitRotation);
                yield return null;
            }
            transform.rotation = Quaternion.LookRotation(direction);

            //then move 
            while (Vector3.Distance(transform.position, destination) >= 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, destination, PlayerConfig.PLAYER_MOVE_SPEED * Time.deltaTime);
                yield return null;
            }
            transform.position = Vector3Int.RoundToInt(destination);

            yield return new WaitForSeconds(PlayerConfig.PLAYER_INVESTIGATE_TIME);

            //rotate back
            while (Vector3.Distance(transform.forward, reverseDirection) >= 0.01f)
            {
                Vector3 splitRotation = Vector3.RotateTowards(transform.forward, reverseDirection, PlayerConfig.PLAYER_ROTATE_SPEED * Time.deltaTime, 0.0f);
                transform.rotation = Quaternion.LookRotation(splitRotation);
                yield return null;
            }
            transform.rotation = Quaternion.LookRotation(reverseDirection);

            //move back
            while (Vector3.Distance(transform.position, origin) >= 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, origin, PlayerConfig.PLAYER_MOVE_SPEED * Time.deltaTime);
                yield return null;
            }
            transform.position = Vector3Int.RoundToInt(origin);

            //rotate to origin direction
            while (Vector3.Distance(transform.forward, originDirection) >= 0.01f)
            {
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
            DrawDebugRay();
        }

        void DrawDebugRay()
        {
            Debug.DrawRay(transform.position, transform.forward * MapConfig.TILE_SCALE / 2, Color.red);
            Debug.DrawRay(transform.position, Quaternion.Euler(0, -90, 0) * transform.forward * MapConfig.TILE_SCALE / 2, Color.blue);
            Debug.DrawRay(transform.position, Quaternion.Euler(0, 90, 0) * transform.forward * MapConfig.TILE_SCALE / 2, Color.blue);
        }
    }
}