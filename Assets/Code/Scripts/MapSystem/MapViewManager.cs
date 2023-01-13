using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using GlobalConfig;

namespace Unity.Game.Map
{
    public class MapViewManager : MonoBehaviour
    {
        // Start is called before the first frame update

        //Virtual Camera Plane
        [SerializeField] private GameObject MapCenterObj;
        [SerializeField] private GameObject MapCameraObj;
        [SerializeField] private GameObject PlayerCameraObj;
        [SerializeField] private GameObject BirdEyeCameraObj;
        [SerializeField] private bool isMapRotating;
        [SerializeField] private float mapWidth;
        [SerializeField] private float mapHeight;
        [SerializeField] private float maxView;

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
                StartCoroutine(RotateLeft());
            }
            if (Input.GetKeyDown(KeyCode.RightBracket))
            {
                StartCoroutine(RotateRight());
            }
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                ToggleBirdEye();
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

            maxView = (mapWidth / 0.63f) / ((mapWidth / 0.63f) / (mapHeight / 0.73f)) / 2;
            MapCameraObj.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = maxView;

            BirdEyeCameraObj.GetComponent<CinemachineVirtualCamera>().Follow = MapCenterObj.transform;
            MapCameraObj.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = maxView;
        }

        private void SetIsMapRotating(bool isMapRotating)
        {
            this.isMapRotating = isMapRotating;
        }
        IEnumerator RotateLeft()
        {
            if (isMapRotating) yield break;

            SetIsMapRotating(true);
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

        //IEnumerator RestoreDefaultView()
        //{
        //    yield return null;
        //    SetIsMapRotating(true);
        //    Vector3 direction = Quaternion.Euler(45, 45, 0) * Vector3.one;
        //    while (Vector3.Distance(MapCameraObj.transform.forward, direction) >= 0.01f)
        //    {
        //        Vector3 splitRotation = Vector3.RotateTowards(MapCameraObj.transform.forward, direction, MapConfig.MAP_ROTATE_SPEED * Time.deltaTime, 0.0f);
        //        MapCameraObj.transform.rotation = Quaternion.LookRotation(splitRotation);
        //        yield return null;
        //    }
        //    MapCameraObj.transform.rotation = Quaternion.LookRotation(direction);
        //    SetIsMapRotating(false);
        //}

        public IEnumerator ViewPlayerMove()
        {
            while (isMapRotating) yield return null;

            PlayerCameraObj.GetComponent<CinemachineVirtualCamera>().Priority = 12;
            BirdEyeCameraObj.GetComponent<CinemachineVirtualCamera>().Priority = 0;

            yield return new WaitForSeconds(3f);
            MapCameraObj.transform.rotation = Quaternion.Euler(45, 45, 0);
            PlayerCameraObj.GetComponent<CinemachineVirtualCamera>().Priority = 0;

        }

        public void ToggleBirdEye()
        {

            if (BirdEyeCameraObj.GetComponent<CinemachineVirtualCamera>().Priority == 0)
            {
                BirdEyeCameraObj.GetComponent<CinemachineVirtualCamera>().Priority = 11;
            }
            else
            {
                BirdEyeCameraObj.GetComponent<CinemachineVirtualCamera>().Priority = 0;
            }

        }
    }
}