using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using GlobalConfig;

namespace Unity.Game.MapSystem
{
    public class MapViewManager : MonoBehaviour
    {
        // Start is called before the first frame update
        public enum RotateDirection { CLOCKWISE, COUNTERCLOCKWISE, BIRDEYE, ORIGIN }

        //public static event Action<GameObject> EnabledBirdEye;
        //public static event Action DisableBirdEye;
        //Virtual Camera Plane
        [SerializeField] private GameObject MapCenterObj;
        [SerializeField] private GameObject MapCameraObj;
        [SerializeField] private GameObject PlayerCameraObj;
        [SerializeField] private GameObject BirdEyeCameraObj;
        [SerializeField] private bool isMapRotating;
        [SerializeField] private float mapWidth;
        [SerializeField] private float mapHeight;
        [SerializeField] private float maxView;
        [SerializeField] private Quaternion OriginalRotation;

        Coroutine RotatingCoroutine;

        public static MapViewManager Instance { get; private set; }
        void Start()
        {

        }

        private void Awake()
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

        // Update is called once per frame
        void Update()
        {
            //test rotate camera
            if (Input.GetKeyDown(KeyCode.LeftBracket))
            {
                StartRotate(RotateDirection.CLOCKWISE);
            }
            if (Input.GetKeyDown(KeyCode.RightBracket))
            {
                StartRotate(RotateDirection.COUNTERCLOCKWISE);
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                StartRotate(RotateDirection.ORIGIN);
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                StartRotate(RotateDirection.BIRDEYE);
            }
        }

        public void GetMapCenter(float x, float y)
        {
            if (!MapCenterObj)
            {
                MapCenterObj = new GameObject("MapCenter");
                MapCenterObj.transform.SetParent(transform);
            }

            MapCameraObj.GetComponent<CinemachineVirtualCamera>().Follow = MapCenterObj.transform;
            MapCenterObj.transform.position = new Vector3((x - 1) * MapConfig.TILE_SCALE / 2, 0, (y - 1) * MapConfig.TILE_SCALE / 2);
            mapWidth = x * MapConfig.TILE_SCALE;
            mapHeight = y * MapConfig.TILE_SCALE;

            BoxCollider mapCollider = MapCenterObj.AddComponent<BoxCollider>();
            mapCollider.size = new Vector3(mapWidth, 0, mapHeight);

            if (mapWidth > mapHeight)
            {
                maxView = (mapWidth / 0.63f) / ((mapWidth / 0.63f) / (mapWidth / 0.73f)) / 2;
            }
            else
            {
                maxView = (mapHeight / 0.63f) / ((mapHeight / 0.63f) / (mapHeight / 0.73f)) / 2;
            }

            MapCameraObj.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = maxView;

            BirdEyeCameraObj.GetComponent<CinemachineVirtualCamera>().Follow = MapCenterObj.transform;
            BirdEyeCameraObj.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = maxView;

            //set OriginalRotation

            //OriginalRotation = MapCameraObj.transform.rotation;
        }

        private void SetIsMapRotating(bool isMapRotating)
        {
            this.isMapRotating = isMapRotating;
        }

        public void StartRotate(RotateDirection direction)
        {
            //if (RotatingCoroutine != null) return;

            switch (direction)
            {
                case RotateDirection.CLOCKWISE:
                    RotatingCoroutine = StartCoroutine(RotateLeft());
                    break;
                case RotateDirection.COUNTERCLOCKWISE:
                    RotatingCoroutine = StartCoroutine(RotateRight());
                    break;
                case RotateDirection.ORIGIN:
                    RotatingCoroutine = StartCoroutine(RestoreDefaultView());
                    break;
                case RotateDirection.BIRDEYE:
                    RotatingCoroutine = StartCoroutine(EnableBirdEye());
                    break;
                default: break;
            }
        }

        IEnumerator RotateLeft()
        {
            if (isMapRotating) yield break;

            SetIsMapRotating(true);

            if (BirdEyeCameraObj.GetComponent<CinemachineVirtualCamera>().Priority != 0)
            {
                BirdEyeCameraObj.GetComponent<CinemachineVirtualCamera>().Priority = 0;
                //MapCameraObj.transform.rotation = Quaternion.Euler(45, 45, 0);
                //yield return new WaitForSeconds(.5f);
            }

            Vector3 direction = Quaternion.Euler(0, -90, 0) * MapCameraObj.transform.forward;
            while (Vector3.Distance(MapCameraObj.transform.forward, direction) >= 0.01f)
            {
                Vector3 splitRotation = Vector3.RotateTowards(MapCameraObj.transform.forward, direction, MapConfig.MAP_ROTATE_SPEED * Time.deltaTime, 0.0f);
                MapCameraObj.transform.rotation = Quaternion.LookRotation(splitRotation);
                yield return null;
            }
            MapCameraObj.transform.rotation = Quaternion.LookRotation(direction);
            SetIsMapRotating(false);

        }

        IEnumerator RotateRight()
        {
            if (isMapRotating) yield break;

            SetIsMapRotating(true);

            if (BirdEyeCameraObj.GetComponent<CinemachineVirtualCamera>().Priority != 0)
            {
                BirdEyeCameraObj.GetComponent<CinemachineVirtualCamera>().Priority = 0;
                //MapCameraObj.transform.rotation = Quaternion.Euler(45, 45, 0);
                //yield return new WaitForSeconds(.5f);
            }
            Vector3 direction = Quaternion.Euler(0, 90, 0) * MapCameraObj.transform.forward;
            while (Vector3.Distance(MapCameraObj.transform.forward, direction) >= 0.01f)
            {
                Vector3 splitRotation = Vector3.RotateTowards(MapCameraObj.transform.forward, direction, MapConfig.MAP_ROTATE_SPEED * Time.deltaTime, 0.0f);
                MapCameraObj.transform.rotation = Quaternion.LookRotation(splitRotation);
                yield return null;
            }
            MapCameraObj.transform.rotation = Quaternion.LookRotation(direction);
            SetIsMapRotating(false);
        }

        IEnumerator RestoreDefaultView()
        {
            if (isMapRotating) yield break;
            SetIsMapRotating(true);
            BirdEyeCameraObj.GetComponent<CinemachineVirtualCamera>().Priority = 0;
            MapCameraObj.transform.rotation = Quaternion.Euler(45, 45, 0);
            SetIsMapRotating(false);
        }

        public IEnumerator ViewPlayerMove()
        {
            while (isMapRotating) yield return null;

            PlayerCameraObj.GetComponent<CinemachineVirtualCamera>().Priority = 12;
            BirdEyeCameraObj.GetComponent<CinemachineVirtualCamera>().Priority = 0;

            yield return new WaitForSeconds(3f);
            MapCameraObj.transform.rotation = Quaternion.Euler(45, 45, 0);
            PlayerCameraObj.GetComponent<CinemachineVirtualCamera>().Priority = 0;

        }

        public IEnumerator EnableBirdEye()
        {
            while (isMapRotating) yield break;
            SetIsMapRotating(true);
            //MapCameraObj.transform.rotation = Quaternion.Euler(45, 45, 0);
            //yield return new WaitForSeconds(.1f);

            if (BirdEyeCameraObj.GetComponent<CinemachineVirtualCamera>().Priority == 0)
            {
                BirdEyeCameraObj.GetComponent<CinemachineVirtualCamera>().Priority = 11;
            }
            else
            {
                BirdEyeCameraObj.GetComponent<CinemachineVirtualCamera>().Priority = 0;
            }
            yield return new WaitForSeconds(1.5f);
            MapCameraObj.transform.rotation = Quaternion.Euler(45, 45, 0);

            SetIsMapRotating(false);
        }

        public void RotateClockWise()
        {
            StartRotate(RotateDirection.CLOCKWISE);
        }

        public void RotateCounterClockWise()
        {
            StartRotate(RotateDirection.COUNTERCLOCKWISE);
        }

        public void RotateOrigin()
        {
            StartRotate(RotateDirection.ORIGIN);
        }

        public void RotateBirdEye()
        {
            StartRotate(RotateDirection.BIRDEYE);
        }

        public bool IsBirdEyeEnabled()
        {
            return BirdEyeCameraObj.GetComponent<CinemachineVirtualCamera>().Priority != 0;
        }
        //public void ToggleBirdEye()
        //{

        //    if (BirdEyeCameraObj.GetComponent<CinemachineVirtualCamera>().Priority == 0)
        //    {
        //        BirdEyeCameraObj.GetComponent<CinemachineVirtualCamera>().Priority = 11;
        //    }
        //    else
        //    {
        //        BirdEyeCameraObj.GetComponent<CinemachineVirtualCamera>().Priority = 0;
        //    }

        //}
    }
}