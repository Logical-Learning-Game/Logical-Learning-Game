
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions;

public class Linkable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private UILineRenderer lineDrawer;
    
    [SerializeField]
    private List<GameObject> linkedAnchor;
    // Start is called before the first frame update
    void Awake()
    {
        lineDrawer.Points = new Vector2[2];
        lineDrawer.Points[0] = new Vector2(0,0);
        lineDrawer.lineThickness = 20f;
        Debug.Log("Linkable Awake");
        //gameObject.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("pointerenter");
        //gameObject.GetComponentInParent<Draggable>().SetisDraggable(false);
        //if (eventData.pointerDrag)
        //{
        //    GameObject linkAnchor = eventData.pointerDrag.GetComponent<GameObject>();
        //    if (linkAnchor)
        //    {
        //        Debug.Log(linkAnchor);
        //    }
        //}
        //throw new System.NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("pointerexit");
        //gameObject.GetComponentInParent<Draggable>().SetisDraggable(true);
        //throw new System.NotImplementedException();
    }


    // Update is called once per frame
    void LateUpdate()
    {
        UpdateLinePosition();
        lineDrawer.SetVerticesDirty();
    }

    void UpdateLinePosition()
    {
        if (gameObject.GetComponent<AbstractCommand>().nextCommand)
        {
            //lineDrawer.Points = new Vector2[] { gameObject.transform.position, gameObject.GetComponent<AbstractCommand>().nextCommand.transform.position };
            
            lineDrawer.Points[1] = gameObject.GetComponent<AbstractCommand>().nextCommand.transform.position - gameObject.transform.position;
        }
        else
        {
            lineDrawer.Points = new Vector2[2];
        }
    }
}
