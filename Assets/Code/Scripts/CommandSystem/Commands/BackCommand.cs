using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BackCommand : AbstractCommand
{
   
    public override void Execute()
    {
        //actionSequence.text += "Back\n";
        
        ActionManager.Instance.AddSequenceText("Back\n");
        UpdateLink("Success");
        CommandManager.Instance.OnExecute(this);
        

    }

  
}
