using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LeftForwardCommand : AbstractCommand
{
    public LeftForwardCommand(GameObject commandObject) : base(commandObject)
    {

    }

    public override void Execute()
    {
        //actionSequence.text += "LeftForward\n";
        ActionManager.Instance.AddSequenceText("LeftForward\n");
    }


}
