using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class CommandDropzone : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData data)
    {
        if (data.pointerDrag != null)
        {
            Debug.Log("Dropped object was: " + data.pointerDrag);
        }
    }
}
