using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Game.Conditions;
using Unity.Game.ItemSystem;

namespace Unity.Game.MapSystem
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private bool isOpened = false;
        [SerializeField] private ItemType doorKey;

        public void SetDoorKey(ItemType key)
        {
            doorKey = key;
        }

        public void OpenDoor()
        {
            if (isOpened)
            {
                return;
            }
            // add open animation
        }

        public bool CompareKey(ItemType key)
        {
            return true;
        }

        public ItemType GetKeyType()
        {
            return doorKey;
        }
    }
}