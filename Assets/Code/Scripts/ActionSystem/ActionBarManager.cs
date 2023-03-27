using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Unity.Game.ActionSystem
{
    public class ActionBarManager : MonoBehaviour
    {
        // Start is called before the first frame update
        public static ActionBarManager Instance { get; private set; }
        //private TMP_Text actionSequence;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private GameObject content;

        [SerializeField] private List<GameObject> ActionPrefabs = new List<GameObject>();
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                //actionSequence = GameObject.Find("ActionSequence").GetComponent<TMP_Text>();
                scrollRect = GetComponent<ScrollRect>();
                content = GameObject.Find("ActionContent");
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void ScrollToRight()
        {
            scrollRect.horizontalNormalizedPosition = 1f;
        }
        void ScrollToLeft()
        {
            scrollRect.horizontalNormalizedPosition = 0f;
        }
        public void AddToContent(Action action)
        {

            // temporary solution ,need more implementation
            switch (action.actionName)
            {
                case "Start!": CreateAction(ActionPrefabs[0]); break;
                case "Forward": CreateAction(ActionPrefabs[1]); break;
                case "Left Forward": CreateAction(ActionPrefabs[2]); break;
                case "Right Forward": CreateAction(ActionPrefabs[3]); break;
                case "Back": CreateAction(ActionPrefabs[4]); break;
                case "Condition": CreateAction(ActionPrefabs[5]); break;

                default: break;

            }

            // scroll to rightmost
            ScrollToRight();
        }

        void CreateAction(GameObject prefab)
        {
            GameObject actionIcon = Instantiate(prefab, content.transform, false);
        }
        public void ClearContents()
        {
            foreach (Transform child in content.transform)
            {
                Destroy(child.gameObject);
            }
            // scroll to leftmost
            ScrollToLeft();
        }
    }

}