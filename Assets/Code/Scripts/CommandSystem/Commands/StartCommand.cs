using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Game.Action;
namespace Unity.Game.Command
{

    public class StartCommand : AbstractCommand
    {
        public override IEnumerator Execute()
        {
            // Reset ActionManager
            ActionManager.Instance.ClearAction();
            yield return base.Execute();

        }

        public override void AddAction()
        {
            ActionManager.Instance.AddAction(new Action.StartAction());
        }
    }
}