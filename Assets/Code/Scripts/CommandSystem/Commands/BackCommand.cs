using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Game.Action;

public class BackCommand : BaseMoveCommand
{

    //public override void Execute()
    //{
    //    //actionSequence.text += "Back\n";
    //    ActionManager.Instance.AddSequenceText("Back\n");
    //    UpdateLink("Success");
    //    CommandManager.Instance.OnExecute(this);

    //}
    public override void AddAction()
    {
        ActionManager.Instance.AddAction(new Action.BackAction());
    }


}
