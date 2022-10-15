
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions;

namespace Unity.Game.Command
{
    public class CommandStatus : MonoBehaviour
    {
        public enum Status
        {
            Default,
            Error,
            Success,
            Warning,
            Executing,
            Selected
        }

        public Status status = Status.Default;

        public void SetStatus(Status status)
        {
            this.status = status;
            switch (status)
            {
                case Status.Default:
                    gameObject.GetComponent<UICircle>().color = CommandColor.Default;
                    gameObject.GetComponent<Linkable>().LineDrawerObject.GetComponent<UILineRenderer>().color = CommandColor.Default;
                    break;
                case Status.Error:
                    gameObject.GetComponent<UICircle>().color = CommandColor.Error;
                    gameObject.GetComponent<Linkable>().LineDrawerObject.GetComponent<UILineRenderer>().color = CommandColor.Error;
                    break;
                case Status.Success:
                    gameObject.GetComponent<UICircle>().color = CommandColor.Success;
                    gameObject.GetComponent<Linkable>().LineDrawerObject.GetComponent<UILineRenderer>().color = CommandColor.Success;
                    break;
                case Status.Warning:
                    gameObject.GetComponent<UICircle>().color = CommandColor.Warning;
                    gameObject.GetComponent<Linkable>().LineDrawerObject.GetComponent<UILineRenderer>().color = CommandColor.Warning;
                    break;
                case Status.Executing:
                    gameObject.GetComponent<UICircle>().color = CommandColor.Warning;
                    gameObject.GetComponent<Linkable>().LineDrawerObject.GetComponent<UILineRenderer>().color = CommandColor.Warning;
                    break;
                case Status.Selected:
                    gameObject.GetComponent<UICircle>().color = CommandColor.Selected;
                    gameObject.GetComponent<Linkable>().LineDrawerObject.GetComponent<UILineRenderer>().color = CommandColor.Selected;

                    break;
                default:
                    break;
            }
        }
    }
}