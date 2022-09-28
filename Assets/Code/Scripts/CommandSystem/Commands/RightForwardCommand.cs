using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RightForwardCommand : AbstractCommand
{
   

    public override void Execute()
    {
        //actionSequence.text += "RightForward\n";
        ActionManager.Instance.AddSequenceText("RightForward\n");
        CommandManager.Instance.OnExecute(this);

    }


}
