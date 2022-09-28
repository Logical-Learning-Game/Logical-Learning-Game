using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class StartCommand : AbstractCommand
{
    public override void Execute()
    {
        // Reset ActionManager
        ActionManager.Instance.ClearSequenceText();
        ActionManager.Instance.AddSequenceText("Start!\n");
        CommandManager.Instance.OnExecute(this);
 
    }

}
