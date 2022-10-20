using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions;

namespace Unity.Game.Command
{
    public class Selectable : MonoBehaviour
    {

        private RectTransform selectingObjectRectTransform;

        private void Awake()
        {
            selectingObjectRectTransform = transform as RectTransform;
        }



        // Update is called once per frame
        void Update()
        {
            //check onclick
            //if (!CommandManager.Instance.isExecuting)
            //{
            //    if (Input.GetMouseButtonDown(0))
            //    {
            //        if (RectTransformUtility.RectangleContainsScreenPoint(selectingObjectRectTransform, Input.mousePosition))
            //        {
            //            CommandManager.Instance.SetSelectedCommand(selectingObjectRectTransform.gameObject);
            //            CommandManager.Instance.VerifyCommand();
            //        }
            //    }
            //}

        }


    }
}