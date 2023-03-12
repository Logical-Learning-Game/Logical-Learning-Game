using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Game.MapSystem
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] GameObject ItemObject;
        Dictionary<Tuple<int, int>, GameObject> DoorOnTile = new Dictionary<Tuple<int, int>, GameObject>() { };
        [SerializeField] GameObject AuraFx;
        [SerializeField] Color AuraColor;
        // Start is called before the first frame update
        void Awake()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public virtual bool IsEnterable(Tuple<int, int> comingDirection)
        {
            CreateTileAura();
            if (GetDoorOnTile(comingDirection) != null)
            {
                Door door = GetDoorOnTile(comingDirection).GetComponent<Door>();
                if (door.IsOpened())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        public virtual void OnTileEntered()
        {

            Debug.Log("Character Entered Tile" + gameObject.name);
        }

        public void SetItemObject(GameObject itemObject)
        {
            ItemObject = itemObject;
        }

        public GameObject GetItemObject()
        {
            return ItemObject;
        }

        public void AddDoorOnTile(Tuple<int, int> doorPos, GameObject door)
        {
            //Debug.Log("AddDoorOnTile->" + doorPos + ":" + door.name);
            DoorOnTile.Add(doorPos, door);
        }

        public GameObject GetDoorOnTile(Tuple<int, int> direction)
        {
            return DoorOnTile.TryGetValue(direction, out GameObject door) ? door : null;
        }

        public void CreateTileAura()
        {
            Debug.Log("Creating Aura");
            if (AuraFx != null)
            {
                GameObject tileAura = Instantiate(AuraFx, transform);
                var main = tileAura.GetComponent<ParticleSystem>().main;
                main.startColor = AuraColor;
                Destroy(tileAura, main.duration+1f);
            }

        }
    }
}