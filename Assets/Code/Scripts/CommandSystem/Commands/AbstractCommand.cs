using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public abstract class AbstractCommand
{
    private GameObject commandObject;
    public AbstractCommand linkedCommand = null;
    public AbstractCommand(GameObject commandObject)
    {
        this.commandObject = commandObject;
    }

    public void SetActive(bool active)
    {
        commandObject.SetActive(active);
    }

    public abstract void Execute();

    public void LinkTo(AbstractCommand linkedCommand)
    {
        this.linkedCommand = linkedCommand;
    }

    public GameObject GetCommandObject()
    {
        return commandObject;
    }

}
