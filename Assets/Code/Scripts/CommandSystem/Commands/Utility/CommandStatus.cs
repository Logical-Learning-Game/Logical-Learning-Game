
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

        public static Dictionary<Status, Color> statusToColor = new Dictionary<Status, Color>() {
            {Status.Default, CommandColor.Default},
            {Status.Error, CommandColor.Error},
            {Status.Success, CommandColor.Success},
            {Status.Warning, CommandColor.Warning},
            {Status.Executing, CommandColor.Warning},
            {Status.Selected, CommandColor.Selected}
        };
   

        public Status status = Status.Default;

        public void SetStatus(Status status)
        {
            this.status = status;
            gameObject.GetComponent<AbstractCommand>().OnStatusChange();
            
        }
        
        public Status GetStatus()
        {
            return status;
        }
    }
}