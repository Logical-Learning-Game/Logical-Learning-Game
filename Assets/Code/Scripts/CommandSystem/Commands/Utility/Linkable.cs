
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions;

namespace Unity.Game.Command
{
    public class Linkable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField]
        protected GameObject lineDrawerPrefab;

        [SerializeField]
        protected GameObject lineDrawerObject;

        public bool isLinking = false;
        public bool isLinkable = true;

        [SerializeField]
        protected Vector2 linkPosition;

        protected Dictionary<string, Color> LinkColor = new Dictionary<string, Color>() {
        { "Default", Color.white },
        { "Disabled", Color.grey },
        { "Success", Color.green },
        { "Error", Color.red },
        { "Executing", Color.yellow }
    };

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            if (isLinkable)
            {
                if (eventData.button == PointerEventData.InputButton.Right)
                {
                    SetisLinking(true);
                    gameObject.GetComponentInParent<AbstractCommand>().Unlink();
                    linkPosition = eventData.position;
                }
            }

        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            if (isLinking) linkPosition = eventData.position;
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            if (isLinking)
            {
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
                SetisLinking(false);
            }
        }

        // Start is called before the first frame update
        protected virtual void Awake()
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
        protected void LateUpdate()
        {
            UpdateLine();
            lineDrawerObject.transform.position = gameObject.transform.position;
            lineDrawerObject.GetComponent<UILineRenderer>().SetVerticesDirty();

            if (CommandManager.Instance.isExecuting)
            {
                SetisLinkable(false);
            }
            else
            {
                SetisLinkable(true);
            }
        }

        protected virtual void UpdateLine()
        {
            if (isLinking)
            {
                lineDrawerObject.GetComponent<UILineRenderer>().Points[1] = linkPosition - (Vector2)gameObject.transform.position;
                SetLinkColor("Default");
            }
            else if (gameObject.GetComponentInParent<AbstractCommand>().GetNextCommand())
            {
                lineDrawerObject.GetComponent<UILineRenderer>().Points[1] = gameObject.GetComponentInParent<AbstractCommand>().GetNextCommand().transform.position - gameObject.transform.position;
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

        public void SetisLinkable(bool isLinkable)
        {
            this.isLinkable = isLinkable;
        }

        public void SetisLinking(bool isLinking)
        {

            this.isLinking = isLinking;

        }
    }
}