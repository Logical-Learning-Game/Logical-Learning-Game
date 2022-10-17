using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.Game.Conditions
{

    public class ConditionPickerController : MonoBehaviour
    {

        public static ConditionPickerController Instance { get; private set; }
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
                foreach (Image condition in GetComponentsInChildren<Image>())
                {
                    if (RectTransformUtility.RectangleContainsScreenPoint(condition.transform as RectTransform, Input.mousePosition))
                    {
                        Debug.Log("Clicked on condition name: "+condition.gameObject.name);
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
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }

}