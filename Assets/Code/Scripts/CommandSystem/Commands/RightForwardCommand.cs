using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Game.Action;

public class RightForwardCommand : BaseMoveCommand
{


    //public override void Execute()
    //{
    //    //actionSequence.text += "RightForward\n";
    //    ActionManager.Instance.AddSequenceText("RightForward\n");
    //    UpdateLink("Success");
    //    CommandManager.Instance.OnExecute(this);

    //}

    public override void AddAction()
    {
        ActionManager.Instance.AddAction(new Action.RightForwardAction());
    }
}
