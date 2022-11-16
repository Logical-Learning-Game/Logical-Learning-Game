using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Game.Conditions;

namespace Unity.Game.Action
{
    public class Action
    {
        public string actionName;
        public virtual IEnumerator Execute()
        {
            yield return new WaitForSeconds(1);
        }
        
    }

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
        Condition condition;
        public ConditionAction(Condition condition)
        {
            actionName = "Condition";
            this.condition = condition;
        }

        public override IEnumerator Execute()
        {
            // mocking a conditionchecker
            condition.SetResult(true);
            return base.Execute();
        }
    }

}