using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI.Extensions;

namespace Unity.Game.Command
{
    public abstract class AbstractCommand : MonoBehaviour
    {
        public static HashSet<System.Type> UnlinkableTypes = new HashSet<System.Type>() {
            typeof(LinkerCommand),
            typeof(StartCommand)
        };

        public AbstractCommand nextCommand = null;
        public List<AbstractCommand> previousCommand = new List<AbstractCommand>();
        public CommandStatus status;
        protected virtual void Awake()
        {
            status = gameObject.AddComponent<CommandStatus>();
 
            //Debug.Log("AbstractCommand Awake");

        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        public virtual IEnumerator Execute()
        {
            Debug.Log("Command Executing");
            status.SetStatus(CommandStatus.Status.Executing);

            yield return AddAction();
            Debug.Log("Executing Complete");
            status.SetStatus(CommandStatus.Status.Success);
            CommandManager.Instance.OnExecute(this);
        }

        public abstract IEnumerator AddAction();

        public void StartExecute()
        {
            StartCoroutine(Execute());
        }

        public virtual void LinkTo(AbstractCommand nextCommand)
        {
            if (!UnlinkableTypes.Contains(nextCommand.GetType()))
            {
                this.nextCommand = nextCommand;
                nextCommand.previousCommand.Add(this);
            }
            else
            {
                Debug.Log("Cannot Link to UnlinkableType Command");
            }

        }

        public virtual void Unlink()
        {
            if (nextCommand != null)
            {
                nextCommand.previousCommand.Remove(this);
                nextCommand = null;
            }
        }


        public virtual void Delete()
        {
            if (nextCommand != null)
            {
                nextCommand.previousCommand.Remove(this);
                nextCommand = null;
            }
            foreach (AbstractCommand command in previousCommand)
            {
                command.nextCommand = null;
            }
            previousCommand.Clear();
            gameObject.SetActive(false);
            gameObject.GetComponentInChildren<Linkable>().OnDelete();
        }

        public virtual void OnStatusChange()
        {
            gameObject.GetComponent<UICircle>().color = CommandStatus.statusToColor[status.GetStatus()];
            gameObject.GetComponent<Linkable>().LineDrawerObject.GetComponent<UILineRenderer>().color = CommandStatus.statusToColor[status.GetStatus()];
        }

        public virtual void OnVerify()
        {
            // set status from error to default after verified
            status.SetStatus(CommandStatus.Status.Default);
        }

        public virtual List<AbstractCommand> GetAllNextCommands()
        {
            List<AbstractCommand> AllNextCommands = new List<AbstractCommand>() { };
            if (nextCommand != null)
            {
                AllNextCommands.Add(nextCommand);
            }

            return AllNextCommands;
        }
    }
}