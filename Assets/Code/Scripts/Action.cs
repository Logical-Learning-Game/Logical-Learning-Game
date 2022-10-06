using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Unity.Game.Action
{
    public class Action
    {
        public string actionName;

        public class StartAction : Action
        {
            public StartAction()
            {
                actionName = "Start!";
            }
        }
        public class ForwardAction : Action
        {
            public ForwardAction()
            {
                actionName = "Forward";
            }
        }

        public class LeftForwardAction : Action
        {
            public LeftForwardAction()
            {
                actionName = "Left Forward";
            }
        }

        public class RightForwardAction : Action
        {
            public RightForwardAction()
            {
                actionName = "Right Forward";
            }
        }

        public class BackAction : Action
        {
            public BackAction()
            {
                actionName = "Back";
            }
        }
        public class ConditionAction : Action
        {
            public ConditionAction()
            {
                actionName = "Condition";
            }

        }
    }

}