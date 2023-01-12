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
        [SerializeField] private bool isMapRotating;

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
        }

        public GameObject GetMapCenter(float x, float y)
        {
            if (!MapCenterObj)
            {
                GameObject MapCenter = new GameObject("MapCenter");
                MapCenterObj = Instantiate(MapCenter);

            }
            if (!MapCameraObj)
            {
                MapCameraObj = GameObject.Find("MapLooker");
            }

            MapCameraObj.GetComponent<CinemachineVirtualCamera>().Follow = MapCenterObj.transform;
            Debug.Log(MapCameraObj);
            MapCenterObj.transform.position = new Vector3((x - 1) * MapConfig.TILE_SCALE / 2, 0, (y - 1) * MapConfig.TILE_SCALE / 2);
            return MapCenterObj;
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

        IEnumerator RestoreDefaultView()
        {
            yield return null;
            SetIsMapRotating(true);
            Vector3 direction = Quaternion.Euler(45, 45, 0) * Vector3.one;
            while (Vector3.Distance(MapCameraObj.transform.forward, direction) >= 0.01f)
            {
                Vector3 splitRotation = Vector3.RotateTowards(MapCameraObj.transform.forward, direction, MapConfig.MAP_ROTATE_SPEED * Time.deltaTime, 0.0f);
                MapCameraObj.transform.rotation = Quaternion.LookRotation(splitRotation);
                yield return null;
            }
            MapCameraObj.transform.rotation = Quaternion.LookRotation(direction);
            SetIsMapRotating(false);
        }

        public IEnumerator ViewPlayerMove()
        {
            while (isMapRotating) yield return null;

            PlayerCameraObj.GetComponent<CinemachineVirtualCamera>().Priority = 12;

            yield return new WaitForSeconds(3f);
            MapCameraObj.transform.rotation = Quaternion.Euler(45, 45, 0);
            PlayerCameraObj.GetComponent<CinemachineVirtualCamera>().Priority = 0;

        }
    }
}