using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class StartCommand : AbstractCommand
{
    public StartCommand(GameObject commandObject) : base(commandObject)
    {
        
    }
    public override void Execute()
    {
        //actionSequence.text += "\nStart!\n";
        ActionManager.Instance.ClearSequenceText();
        ActionManager.Instance.AddSequenceText("Start!\n");
    }

}
