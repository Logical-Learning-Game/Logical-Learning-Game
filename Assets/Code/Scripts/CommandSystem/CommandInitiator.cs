using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CommandInitiator : MonoBehaviour , IBeginDragHandler , IDragHandler
{
    public GameObject commandPrefab;
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("Begin Drag");
        CommandManager.Instance.InstantiateCommand(commandPrefab, eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
 
    }
}
