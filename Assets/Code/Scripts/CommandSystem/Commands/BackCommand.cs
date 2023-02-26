using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Game.ActionSystem;

namespace Unity.Game.Command
{
    public class BackCommand : BaseMoveCommand
    {
        protected override void Awake()
        {
            base.Awake();

            SetCommandType(CommandType.BACK);
        }

        public override IEnumerator AddAction()
        {
            yield return ActionManager.Instance.AddAction(new BackAction());
        }

    }
}