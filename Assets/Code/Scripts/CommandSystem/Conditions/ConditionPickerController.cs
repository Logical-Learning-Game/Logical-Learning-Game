using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Game.Command;

namespace Unity.Game.Conditions
{

    public class ConditionPickerController : MonoBehaviour
    {

        public static ConditionPickerController Instance { get; private set; }

        private ConditionCommand selectingCommand;
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                gameObject.SetActive(false);
            }
            else
            {
                Destroy(gameObject);
            }

        }

        // Update is called once per frame
        void Update()
        {
            OnPickCondition();
        }

        void OnPickCondition()
        {
            if (Input.GetMouseButtonDown(0))
            {
                foreach (ConditionChoice choice in GetComponentsInChildren<ConditionChoice>())
                {
                    if (RectTransformUtility.RectangleContainsScreenPoint(choice.transform as RectTransform, Input.mousePosition))
                    {
                        Debug.Log("Clicked on condition name: "+choice.gameObject.name);
                        selectingCommand.SetCondition(choice.Condition);
                    }

                    //if (!RectTransformUtility.RectangleContainsScreenPoint(transform as RectTransform, Input.mousePosition))
                    //{
                    //    Close();
                    //}
                    
                }
                Close();
            }
        }

        public void Open(Transform transform)
        {
            gameObject.SetActive(true);
            gameObject.transform.position = transform.position;
            selectingCommand = transform.GetComponentInParent<ConditionCommand>();
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }

}