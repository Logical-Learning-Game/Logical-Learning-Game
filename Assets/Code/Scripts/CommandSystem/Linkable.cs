
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions;

public class Linkable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    private GameObject lineDrawerPrefab;

    [SerializeField]
    private GameObject lineDrawerObject;

    [SerializeField]
    private bool isLinking = false;
    [SerializeField]
    private Vector2 linkPosition;

    private Dictionary<string, Color> LinkColor = new Dictionary<string, Color>() {
        { "Default", Color.white },
        { "Disabled", Color.grey },
        { "Success", Color.green },
        { "Error", Color.red },
        { "Looping", Color.yellow }
    };

    public void OnBeginDrag(PointerEventData eventData)
    {

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            isLinking = true;
            gameObject.GetComponentInParent<AbstractCommand>().Unlink();
            linkPosition = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("on drag");
        linkPosition = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("end drag");
        isLinking = false;
        Debug.Log(eventData.pointerEnter.name);
        //if pointer is over a linkable object
        if (eventData.pointerEnter != null && eventData.pointerEnter.GetComponent<Linkable>() != null)
        {
            //if the object is not the same as this object
            if (eventData.pointerEnter != gameObject)
            {
                //Debug.Log(eventData.pointerEnter.name);
                //if the object is not already linked to this object
                if (!eventData.pointerEnter.GetComponentInParent<AbstractCommand>().previousCommand.Contains(gameObject.GetComponentInParent<AbstractCommand>()))
                {
                    //link the two objects
                    gameObject.GetComponentInParent<AbstractCommand>().Unlink();
                    gameObject.GetComponentInParent<AbstractCommand>().LinkTo(eventData.pointerEnter.GetComponentInParent<AbstractCommand>());
                    
                }
            }
        }
        CommandManager.SaveCommandState();
        CommandManager.Instance.VerifyCommand();
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (!lineDrawerObject)
        {
            lineDrawerObject = Instantiate(lineDrawerPrefab, transform.position, Quaternion.identity);
            UILineRenderer lineDrawer = lineDrawerObject.GetComponent<UILineRenderer>();
            lineDrawer.Points = new Vector2[2] { Vector2.zero, Vector2.zero };
            lineDrawer.Points[0] = new Vector2(0, 0);
            lineDrawer.lineThickness = 20f;
            Debug.Log("Linkable Awake");

            lineDrawerObject.transform.SetParent(GameObject.Find("LineDrawers").transform);
        }

        //gameObject.SetActive(false);
    }


    // Update is called once per frame
    void LateUpdate()
    {
        UpdateLine();
        lineDrawerObject.transform.position = gameObject.transform.position;
        lineDrawerObject.GetComponent<UILineRenderer>().SetVerticesDirty();
    }

    void UpdateLine()
    {
        if (isLinking)
        {
            lineDrawerObject.GetComponent<UILineRenderer>().Points[1] = linkPosition - (Vector2)gameObject.transform.position;
            SetLinkColor("Default");
        }
        else if (gameObject.GetComponentInParent<AbstractCommand>().nextCommand)
        {
            lineDrawerObject.GetComponent<UILineRenderer>().Points[1] = gameObject.GetComponentInParent<AbstractCommand>().nextCommand.transform.position - gameObject.transform.position;
        }
        else
        {
            lineDrawerObject.GetComponent<UILineRenderer>().Points[1] = Vector2.zero;
        }
    }

    public void SetLinkColor(string color)
    {
        lineDrawerObject.GetComponent<UILineRenderer>().color = LinkColor[color];
    }
}
