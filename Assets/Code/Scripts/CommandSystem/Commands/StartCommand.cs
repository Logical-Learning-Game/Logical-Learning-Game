using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class StartCommand : AbstractCommand
{
    public override void Execute()
    {
        //actionSequence.text += "\nStart!\n";
        ActionManager.Instance.ClearSequenceText();
        ActionManager.Instance.AddSequenceText("Start!\n");
        nextCommand?.Execute();
    }

}
