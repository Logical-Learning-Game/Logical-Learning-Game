
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions;

namespace Unity.Game.Command
{
    public class ConditionLinkable : Linkable
    {

        public override void OnBeginDrag(PointerEventData eventData)
        {

            if (eventData.button == PointerEventData.InputButton.Right)
            {
                isLinking = true;
                gameObject.GetComponentInParent<AbstractCommand>().Unlink();
                linkPosition = eventData.position;
            }
        }

        public override void OnDrag(PointerEventData eventData)
        {
            linkPosition = eventData.position;
        }

        public override void OnEndDrag(PointerEventData eventData)
        {

            isLinking = false;
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


        protected override void UpdateLine()
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

    }
}