using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Game.Action;
public class ConditionCommand : AbstractCommand
{
    // default nextCommand is false case;
    public AbstractCommand nextCommandIfTrue;
    public AbstractCommand nextCommandIfFalse;
    public Condition commandCondition = new Condition();
    //public override void Execute()
    //{

    //    ActionManager.Instance.AddSequenceText("Condition: ");
    //    if (commandCondition.GetResult())
    //    {
    //        ActionManager.Instance.AddSequenceText("True\n");
    //        nextCommand = nextCommandIfTrue;
    //    }
    //    else
    //    {
    //        ActionManager.Instance.AddSequenceText("False\n");
    //        nextCommand = nextCommandIfFalse;
    //    }
    //    CommandManager.Instance.OnExecute(this);

    //}
    public override void AddAction()
    {
        ActionManager.Instance.AddAction(new Action.ConditionAction());
    }

    public override void LinkTo(AbstractCommand nextCommand)
    {
        //override LinkTo to LinkToFalseCommand
        if (nextCommand.GetType() != typeof(StartCommand))
        {
            nextCommandIfFalse = nextCommand;
            nextCommand.previousCommand.Add(this);
        }
        else
        {
            Debug.Log("Cannot Link to Start Command");
        }

    }

    public override void Unlink()
    {
        if (nextCommandIfFalse != null)
        {
            nextCommandIfFalse.previousCommand.Remove(this);
            nextCommandIfFalse = null;
        }
    }

    public override void UpdateLink(string color)
    {
        gameObject.GetComponentInChildren<Linkable>().SetLinkColor(color);
        // implement another linkable for true case

    }

    public override void SoftRemove()
    {
        if (nextCommandIfFalse != null)
        {
            nextCommandIfFalse.previousCommand.Remove(this);
        }
        foreach (AbstractCommand command in previousCommand)
        {
            command.nextCommand = null;
        }
        previousCommand.Clear();
        gameObject.SetActive(false);
    }

    public void LinkToTrue(AbstractCommand nextCommand)
    {
        if (nextCommand.GetType() != typeof(StartCommand))
        {
            nextCommandIfTrue = nextCommand;
            nextCommand.previousCommand.Add(this);
        }
        else
        {
            Debug.Log("Cannot Link to Start Command");
        }
    }

    public void UnlinkTrue()
    {
        if (nextCommandIfTrue != null)
        {
            nextCommandIfTrue.previousCommand.Remove(this);
            nextCommandIfTrue = null;
        }
    }

    public void UpdateLinkTrue(string color)
    {
        gameObject.GetComponentInChildren<ConditionLinkable>().SetLinkColor(color);
        // implement another linkable for true case

    }

}
