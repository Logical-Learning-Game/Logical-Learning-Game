using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Game.Conditions;
using Unity.Game.Level;

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
        
        public override IEnumerator Execute()
        {
            yield return LevelManager.Instance.OnPlayerMove(Player.Instance.Front());
        }
    }

    public class LeftForwardAction : Action
    {
        public LeftForwardAction()
        {
            actionName = "Left Forward";
        }

        public override IEnumerator Execute()
        {
            yield return LevelManager.Instance.OnPlayerMove(Player.Instance.Left());
        }
    }

    public class RightForwardAction : Action
    {
        public RightForwardAction()
        {
            actionName = "Right Forward";
        }

        public override IEnumerator Execute()
        {
            yield return LevelManager.Instance.OnPlayerMove(Player.Instance.Right());
        }
    }

    public class BackAction : Action
    {
        public BackAction()
        {
            actionName = "Back";
        }

        public override IEnumerator Execute()
        {
            yield return LevelManager.Instance.OnPlayerMove(Player.Instance.Back());
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
            //condition.SetResult(true);
            return base.Execute();
        }
    }

}