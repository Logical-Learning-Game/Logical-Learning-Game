using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Game.Level;

namespace Unity.Game.ItemSystem
{
    public enum ItemType { NONE, KEY_A, KEY_B, KEY_C }
    public class Item : MonoBehaviour
    {
        [SerializeField] ItemType itemType;
        void Start()
        {

        }

        public virtual void OnPickUp()
        {
            //decide between force open door or require key to open door
            LevelManager.Instance.AddItem(this.itemType);
            ItemManager.Instance.RemoveItemFromMap(this);
            AudioManager.PlayPickItemSound();
        }

        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log("Test Coliding");
            if (other.gameObject.CompareTag("Player"))
            {
                //Debug.Log("Found Player Tag");
                OnPickUp();
            }
        }

    }
}

