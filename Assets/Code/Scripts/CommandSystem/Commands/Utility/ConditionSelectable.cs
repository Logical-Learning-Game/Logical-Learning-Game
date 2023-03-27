using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions;
using Unity.Game.Conditions;

namespace Unity.Game.Command
{
    public class ConditionSelectable : MonoBehaviour
    {

        private RectTransform selectingObjectRectTransform;
        private ConditionPickerController picker;
        
        private void Awake()
        {
            selectingObjectRectTransform = transform as RectTransform;
            picker = ConditionPickerController.Instance;
        }



        // Update is called once per frame
        void Update()
        {
            //check onclick
            if (!CommandManager.Instance.isExecuting)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (RectTransformUtility.RectangleContainsScreenPoint(selectingObjectRectTransform, Input.mousePosition))
                    {
                        //CommandManager.Instance.SetSelectedCommand(selectingObjectRectTransform.gameObject);
                        picker.Open(selectingObjectRectTransform);
                    }
                }
            }

        }


    }
}