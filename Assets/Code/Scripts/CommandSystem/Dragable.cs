
using UnityEngine;
using UnityEngine.EventSystems;

public class Dragable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    private float _dragSpeed = 0.09f;
    private RectTransform draggingObjectRectTransform;
    private Vector3 velocity = Vector3.zero;

    public bool isDragable = true;
    
    public bool isDragging = false;

    public Vector2 dragPosition;
    
    private void Awake()
    {
        draggingObjectRectTransform = transform as RectTransform;
        dragPosition = draggingObjectRectTransform.position;
    }
    public void OnDrag(PointerEventData eventData)
    {
        
        if (isDragable && RectTransformUtility.ScreenPointToWorldPointInRectangle(draggingObjectRectTransform, eventData.position, eventData.pressEventCamera, out var globalMousePosition))
        {
            isDragging = true;
            dragPosition = globalMousePosition;
           
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging)
        {
            draggingObjectRectTransform.position = Vector3.SmoothDamp(draggingObjectRectTransform.position, dragPosition, ref velocity, _dragSpeed);
            if (Vector3.Distance(draggingObjectRectTransform.position, dragPosition) < 0.5f)
            {
                draggingObjectRectTransform.position = dragPosition;
                isDragging = false;
            }
        } 

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //check if drop command outside command panel

        CommandManager.SaveCommandState();
        Debug.Log("Saved");
    }

    public void SetisDragable(bool isDragable)
    {
        this.isDragable = isDragable;
    }
}
