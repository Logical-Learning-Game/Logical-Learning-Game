using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
public abstract class AbstractCommand : MonoBehaviour
{

    public AbstractCommand nextCommand = null;
    public List<AbstractCommand> previousCommand = new List<AbstractCommand>();

    private Dictionary<string, Color> LinkColor = new Dictionary<string, Color>() {
        { "Default", Color.white },
        { "Disabled", Color.grey },
        { "Success", Color.green },
        { "Error", Color.red },
        { "Looping", Color.yellow }
    };

    private void Awake()
    {
        Debug.Log("AbstractCommand Awake");
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    //public abstract void Execute();
    public abstract void Execute();

    public void LinkTo(AbstractCommand nextCommand)
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

    public void Unlink()
    {
        if (nextCommand != null)
        {
            nextCommand.previousCommand.Remove(this);
            nextCommand = null;
        }
    }

    public void UpdateLink(string color)
    {
        gameObject.GetComponentInChildren<Linkable>().SetLinkColor(color);
    }

    public void SoftRemove()
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
