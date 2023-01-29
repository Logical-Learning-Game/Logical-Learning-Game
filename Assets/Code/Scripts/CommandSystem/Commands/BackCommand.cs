using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Game.Action;

namespace Unity.Game.Command
{
    public class BackCommand : BaseMoveCommand
    {
        public override IEnumerator AddAction()
        {
            yield return ActionManager.Instance.AddAction(new Action.BackAction());
        }

    }
}