using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LeftForwardCommand : AbstractCommand
{
 

    public override void Execute()
    {
        //actionSequence.text += "LeftForward\n";
        ActionManager.Instance.AddSequenceText("LeftForward\n");
        nextCommand?.Execute();
    }


}
