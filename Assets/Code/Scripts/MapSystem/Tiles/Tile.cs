using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Game.MapSystem
{
    public class Tile : MonoBehaviour
    {
        GameObject ItemObject;
        // Start is called before the first frame update
        void Awake()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        
        public virtual bool IsEnterable()
        {
            return false;
        }

        public virtual void OnTileEntered()
        {
            Debug.Log("Character Entered Tile"+gameObject.name);
        }

        public void SetItemObject(GameObject itemObject)
        {
            ItemObject = itemObject;
        }
        
        public GameObject GetItemObject()
        {
            return ItemObject;
        }
    }
}