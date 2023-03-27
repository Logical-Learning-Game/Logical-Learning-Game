using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions;

namespace Unity.Game.Command
{
    public class CommandInitiator : MonoBehaviour, IBeginDragHandler, IDragHandler
    {
        public GameObject commandPrefab;
        public bool isEnabled;
        [SerializeField]
        private Color commandColor;
        private Color disableColor = Color.gray;

        public void OnBeginDrag(PointerEventData eventData)
        {

            if (isEnabled)
            {
                if (eventData.button == PointerEventData.InputButton.Left)
                {
                    GameObject commandObject = CommandInitiate(eventData);
                    CommandManager.Instance.AddCommand(commandObject);
                    CommandBarManager.Instance.OnUpdateCommandBar();
                    AudioManager.PlayCommandPickSound();
                }
            }

        }

        public GameObject CommandInitiate(PointerEventData eventData)
        {
            GameObject commandObject = Instantiate(commandPrefab, eventData.position, Quaternion.identity);
            Draggable draggableComponent = commandObject.GetComponentInChildren<Draggable>();
            draggableComponent.isDraggable = true;
            commandObject.transform.SetParent(GameObject.Find("CommandBoard").transform);
            eventData.pointerDrag = draggableComponent.gameObject;
            return commandObject;
        }

        public void OnDrag(PointerEventData eventData)
        {

        }

        public void Start()
        {
            OnUpdateCommandBar();
        }

        public void Update()
        {

        }

        public void setEnabled(bool isEnabled)
        {
            this.isEnabled = isEnabled;
        }
        
        public void OnUpdateCommandBar()
        {
            if (isEnabled)
            {
                gameObject.GetComponent<UICircle>().color = commandColor;
            }
            else
            {
                gameObject.GetComponent<UICircle>().color = disableColor;
            }
        }
    }
}