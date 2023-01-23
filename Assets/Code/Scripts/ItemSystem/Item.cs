using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Game.ItemSystem
{
    public enum ItemType { KEY_A, KEY_B, KEY_C }
    public class Item : MonoBehaviour
    {
        [SerializeField] ItemType itemType;
        void Start()
        {

        }

        public virtual void OnPickUp()
        {
            //decide between force open door or require key to open door
            //Player.Instance.AddItem(this.itemType)
            //ItemManager.Instance.RemoveItemFromMap(this); 
            //Destroy(gameObject);
        }


    }
}

