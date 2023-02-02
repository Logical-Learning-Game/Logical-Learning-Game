using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Game.ActionSystem;
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

        public override IEnumerator AddAction()
        {
            yield return ActionManager.Instance.AddAction(new StartAction());
        }
    }
}