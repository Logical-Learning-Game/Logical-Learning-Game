using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RightForwardCommand : AbstractCommand
{
    public RightForwardCommand(GameObject commandObject) : base(commandObject)
    {

    }

    public override void Execute()
    {
        //actionSequence.text += "RightForward\n";
        ActionManager.Instance.AddSequenceText("RightForward\n");
    }


}
