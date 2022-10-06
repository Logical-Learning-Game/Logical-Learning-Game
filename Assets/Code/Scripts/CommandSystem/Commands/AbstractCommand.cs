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

        private void Awake()
        {
            Debug.Log("AbstractCommand Awake");
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        public virtual IEnumerator Execute()
        {
            Debug.Log("Command Executing");
            UpdateLink("Executing");
            AddAction();
            yield return new WaitForSeconds(1);
            Debug.Log("Executing Complete");
            UpdateLink("Success");
            CommandManager.Instance.OnExecute(this);
        }

        public abstract void AddAction();

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

        public virtual void UpdateLink(string color)
        {
            gameObject.GetComponentInChildren<Linkable>().SetLinkColor(color);
        }

        public virtual void SoftRemove()
        {
            if (nextCommand != null)
            {
                nextCommand.previousCommand.Remove(this);
            }
            foreach (AbstractCommand command in previousCommand)
            {
                command.nextCommand = null;
            }
            previousCommand.Clear();
            gameObject.SetActive(false);
        }
    }
}