using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Unity.Game.ItemSystem
{
    public class ItemPanelManager : MonoBehaviour
    {
        public static ItemPanelManager Instance { get; private set; }

        Dictionary<ItemType, GameObject> ItemObjects;

        Dictionary<ItemType, string> ItemTypeString = new Dictionary<ItemType, string>() { { ItemType.KEY_A, "F" }, { ItemType.KEY_B, "H" }, { ItemType.KEY_C, "O" } };
        [SerializeField] private GameObject KeyDisplayPrefab;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                ItemObjects = new Dictionary<ItemType, GameObject>();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void InitItemPanel(HashSet<ItemType> itemTypes)
        {
            //destroy and reset first
            if (ItemObjects == null)
            {
                ItemObjects = new Dictionary<ItemType, GameObject>();
            }
            ItemObjects.Clear();

            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            foreach (ItemType itemType in itemTypes)
            {
                Debug.Log($"Instantiate {itemType}");
                GameObject itemObject = Instantiate(KeyDisplayPrefab, transform, false);
                itemObject.transform.Find("KeyText").GetComponent<TMP_Text>().text = ItemTypeString[itemType];
                ItemObjects.Add(itemType, itemObject);
                SetItemAmount(itemType, 0);
            }

            if(ItemObjects.Count > 0)
            {
                transform.GetComponent<RectTransform>().sizeDelta = new Vector2(50 + itemTypes.Count * 120, 150);
            }
            else
            {
                transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
            }
           
        }


        public void SetItemAmount(ItemType itemType, int amount)
        {
            ItemObjects[itemType].transform.Find("KeyAmount").GetComponent<TMP_Text>().text = $"x{amount}";
        }
    }
}