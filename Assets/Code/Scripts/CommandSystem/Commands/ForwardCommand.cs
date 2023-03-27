using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Game.ActionSystem;
namespace Unity.Game.Command
{
    public class ForwardCommand : BaseMoveCommand
    {
        protected override void Awake()
        {
            base.Awake();

            SetCommandType(CommandType.FORWARD);
        }

        public override IEnumerator AddAction()
        {
            yield return ActionManager.Instance.AddAction(new ForwardAction());
        }

    }
}
