using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Game.Action;

namespace Unity.Game.Command
{
    public class LeftForwardCommand : BaseMoveCommand
    {

        public override void AddAction()
        {
            ActionManager.Instance.AddAction(new Action.LeftForwardAction());
        }

    }
}