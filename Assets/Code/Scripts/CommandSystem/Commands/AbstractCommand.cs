using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
public abstract class AbstractCommand : MonoBehaviour
{

    public AbstractCommand nextCommand = null;
    public AbstractCommand previousCommand = null;

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
        this.nextCommand = nextCommand;
        nextCommand.previousCommand = this;
    }

}
