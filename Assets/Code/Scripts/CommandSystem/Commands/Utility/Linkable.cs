
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

        private GameObject lineDrawerObject;
        public GameObject LineDrawerObject { get => lineDrawerObject; set => lineDrawerObject = value; }
        
        [SerializeField]
        private float ResolutionRatio = 42.5f;
        
        public bool isLinking = false;
        public bool isLinkable = true;

        [SerializeField]
        protected Vector2 linkPosition;


        

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            if (isLinkable)
            {
                if (eventData.button == PointerEventData.InputButton.Right)
                {
                    SetisLinking(true);
                    gameObject.GetComponent<AbstractCommand>().Unlink();
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
                        if (!eventData.pointerEnter.GetComponent<AbstractCommand>().previousCommand.Contains(gameObject.GetComponent<AbstractCommand>()))
                        {
                            //link the two objects
                            gameObject.GetComponent<AbstractCommand>().Unlink();
                            gameObject.GetComponent<AbstractCommand>().LinkTo(eventData.pointerEnter.GetComponent<AbstractCommand>());
                            AudioManager.PlayCommandPickSound();
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
                lineDrawer.lineThickness = 30f;

                lineDrawerObject.transform.SetParent(GameObject.Find("LineDrawers").transform);
            }

            //gameObject.SetActive(false);
        }


        // Update is called once per frame
        protected void Update()
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
            UILineRenderer lineRenderer = lineDrawerObject.GetComponent<UILineRenderer>();
            if (isLinking)
            {
                lineRenderer.Points[0] = Vector2.zero;
                lineRenderer.Points[1] = linkPosition - (Vector2)gameObject.transform.position;
            }
            else if (gameObject.GetComponent<AbstractCommand>().nextCommand != null)
            {
                // check if it is cycle linking 
                AbstractCommand nextCommand = gameObject.GetComponent<AbstractCommand>().nextCommand;
                if (GetComponent<AbstractCommand>().previousCommand.Contains(nextCommand))
                {
                    Vector2 direction = gameObject.GetComponent<AbstractCommand>().nextCommand.transform.position - gameObject.transform.position;
                    direction = direction.normalized;
                    Vector3 rotatedDirection = Quaternion.Euler(0, 0, 90) * direction;

                    lineRenderer.Points[0] = rotatedDirection*25;
                    lineRenderer.Points[1] = gameObject.GetComponent<AbstractCommand>().nextCommand.transform.position - gameObject.transform.position + rotatedDirection * 25;
                }
                else
                {
                    lineRenderer.Points[0] = Vector2.zero;
                    lineRenderer.Points[1] = gameObject.GetComponent<AbstractCommand>().nextCommand.transform.position - gameObject.transform.position;
                }
                
            }
            else
            {
                lineRenderer.Points[1] = Vector2.zero;
            }

            lineRenderer.Resolution = Vector2.Distance(lineRenderer.Points[0], lineRenderer.Points[1]) / ResolutionRatio;
        }



        public void SetisLinkable(bool isLinkable)
        {
            this.isLinkable = isLinkable;
        }

        public void SetisLinking(bool isLinking)
        {

            this.isLinking = isLinking;

        }

        public void OnDelete()
        {
            lineDrawerObject.GetComponent<UILineRenderer>().Points[1] = Vector2.zero;
            lineDrawerObject.SetActive(false);
        }
    }
}