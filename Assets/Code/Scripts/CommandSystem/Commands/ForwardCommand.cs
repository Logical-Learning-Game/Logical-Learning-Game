using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Game.Action;
namespace Unity.Game.Command
{
    public class ForwardCommand : BaseMoveCommand
    {


        public override IEnumerator AddAction()
        {
            yield return ActionManager.Instance.AddAction(new Action.ForwardAction());
        }

    }
}
