using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class CommandInitiator : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    public GameObject commandPrefab;
    public bool isEnabled;
    [SerializeField]
    private Color commandColor;
    [SerializeField]
    private GameObject baseIcon;
    private Color disableColor = Color.gray;

    public void OnBeginDrag(PointerEventData eventData)
    {

        if (isEnabled)
        {
            GameObject commandObject = CommandInitiate(eventData);
            CommandManager.Instance.InstantiateCommand(Initiate(commandObject));
        }

    }

    public abstract AbstractCommand Initiate(GameObject commandObject);
    public GameObject CommandInitiate(PointerEventData eventData)
    {
        GameObject commandObject = Instantiate(commandPrefab, eventData.position, Quaternion.identity);
        commandObject.GetComponent<Dragable>().isDragable = true;
        commandObject.transform.SetParent(gameObject.transform.parent.parent);
        eventData.pointerDrag = commandObject;
        return commandObject;
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void Start()
    {
        if (isEnabled)
        {
            gameObject.GetComponentsInChildren<Image>()[0].color = commandColor;
        }
        else
        {
            gameObject.GetComponentsInChildren<Image>()[0].color = disableColor;
        }
    }

    public void Update()
    {

        if (isEnabled)
        {
            //Debug.Log("Enabled");
            //gameObject.GetComponentsInChildren<Image>()[0].color = commandColor;
            baseIcon.GetComponent<Image>().color = commandColor;
        }
        else
        {
            //Debug.Log("Disabled");
            //gameObject.GetComponentsInChildren<Image>()[0].color = disableColor;
            baseIcon.GetComponent<Image>().color = disableColor;
        }
    }

    public void setEnabled(bool isEnabled)
    {
        this.isEnabled = isEnabled;
    }
}
