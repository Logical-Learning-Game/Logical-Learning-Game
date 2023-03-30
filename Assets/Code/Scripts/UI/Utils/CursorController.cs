using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField] private Texture2D DefaultCursor;
    [SerializeField] private Texture2D LeftClickCursor;
    [SerializeField] private Texture2D RightClickCursor;
    
    private Vector2 hotspot = new Vector2(22f,8f);
    private CursorMode mode = CursorMode.Auto;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.SetCursor(LeftClickCursor, hotspot, mode);
        }
        if (Input.GetMouseButtonDown(1))
        {
            Cursor.SetCursor(RightClickCursor, hotspot, mode);
        }
        if(Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            Cursor.SetCursor(DefaultCursor,hotspot,mode);
        }
    }
}
