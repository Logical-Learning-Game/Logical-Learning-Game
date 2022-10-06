using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Game.Action;
public class LeftForwardCommand : BaseMoveCommand
{

    //public override void Execute()
    //{
    //    //actionSequence.text += "LeftForward\n";
    //    ActionManager.Instance.AddSequenceText("LeftForward\n");
    //    UpdateLink("Success");
    //    CommandManager.Instance.OnExecute(this);

    //}
    public override void AddAction()
    {
        ActionManager.Instance.AddAction(new Action.LeftForwardAction());
    }

}
