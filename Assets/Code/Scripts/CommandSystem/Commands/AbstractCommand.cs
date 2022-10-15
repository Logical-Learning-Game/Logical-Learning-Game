using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using Unity.Game.Action;

namespace Unity.Game.Command
{
    public abstract class AbstractCommand : MonoBehaviour
    {

        public AbstractCommand nextCommand = null;
        public List<AbstractCommand> previousCommand = new List<AbstractCommand>();
        public CommandStatus status;
        private void Awake()
        {
            status = gameObject.GetComponent<CommandStatus>();
            Debug.Log("AbstractCommand Awake");
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

        public virtual AbstractCommand GetNextCommand()
        {
            return nextCommand;
        }

        public virtual void LinkTo(AbstractCommand nextCommand)
        {
            if (nextCommand.GetType() != typeof(StartCommand))
            {
                this.nextCommand = nextCommand;
                nextCommand.previousCommand.Add(this);
            }
            else
            {
                Debug.Log("Cannot Link to Start Command");
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
    }
}