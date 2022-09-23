using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ForwardCommand : AbstractCommand
{

    public override void Execute()
    {
        //actionSequence.text += "Forward\n";
        ActionManager.Instance.AddSequenceText("Forward\n");
        nextCommand?.Execute();
    }

}
