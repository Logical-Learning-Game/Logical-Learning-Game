
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    private float _dragSpeed = 0.09f;

    [SerializeField]
    private RectTransform draggingObjectRectTransform;
    private RectTransform draggableRectTransform;

    private Vector3 velocity = Vector3.zero;

    public bool isDraggable = true;

    public bool isDragging = false;

    public Vector2 dragPosition;

    private void Awake()
    {
        draggingObjectRectTransform = transform.parent.transform as RectTransform;
        dragPosition = draggingObjectRectTransform.position;

        draggableRectTransform = transform as RectTransform;
    }
    public void OnDrag(PointerEventData eventData)
    {

        if (eventData.button == PointerEventData.InputButton.Left && isDraggable)
        {
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(draggableRectTransform, eventData.position, eventData.pressEventCamera, out var globalMousePosition))
            {
                isDragging = true;
                dragPosition = globalMousePosition;
            }
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
                SetisDraggable(false);
            }
        }

        if (CommandManager.Instance.isExecuting)
        {
            SetisDraggable(false);
        }
        else
        {
            SetisDraggable(true);
        }
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        //
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //check if drop command outside command panel
        Debug.Log(eventData.pointerEnter.name);

        CommandManager.SaveCommandState();
    }

    public void SetisDraggable(bool isDraggable)
    {
        this.isDraggable = isDraggable;
    }

    public void SetisDragging(bool isDragging)
    {
        this.isDragging = isDragging;
    }
}
