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

        public IEnumerator TryOpenDoor(List<ItemType> items)
        {
            if (isOpened)
            {
                yield break;
            }
            if(doorKey == ItemType.NONE)
            {
                SetIsOpened(true);
                yield return new WaitForSeconds(0.5f);
                gameObject.SetActive(false);
            }
            else if (items.Contains(doorKey))
            {
                //open door
                SetIsOpened(true);
                items.Remove(doorKey);
                //implement open door animation later
                yield return new WaitForSeconds(0.5f);
                gameObject.SetActive(false);
            }
            else
            {
                //play sound
                yield return new WaitForSeconds(0.5f);
            }
        }


        public ItemType GetKeyType()
        {
            return doorKey;
        }

        public bool IsOpened()
        {
            return isOpened;
        }

        public void SetIsOpened(bool isOpened)
        {
            this.isOpened = isOpened;
        }
    }
}