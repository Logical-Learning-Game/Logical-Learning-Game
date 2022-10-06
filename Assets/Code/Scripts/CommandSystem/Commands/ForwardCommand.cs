using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Game.Action;

public class ForwardCommand : BaseMoveCommand
{

    //public override void Execute()
    //{
    //    //actionSequence.text += "Forward\n";
    //    ActionManager.Instance.AddSequenceText("Forward\n");
    //    UpdateLink("Success");
    //    CommandManager.Instance.OnExecute(this);

    //}

    public override void AddAction()
    {
        ActionManager.Instance.AddAction(new Action.ForwardAction());
    }

}
