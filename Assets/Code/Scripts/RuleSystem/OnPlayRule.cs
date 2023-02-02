using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalConfig;
using System;
using Unity.Game.ActionSystem;
using Unity.Game;

namespace Unity.Game.RuleSystem
{

    public class OnPlayRule : Rule
    {
        uint value;
        bool isMore;
        public OnPlayRule(string id = "test-onclear", RuleTheme theme = RuleTheme.NORMAL, uint value = 0, bool isMore = false) : base(id, theme)
        {
            this.value = value;
            this.isMore = isMore;
        }
        public override string GetDescription()
        {
            return base.GetDescription();
        }

        protected uint GetValue()
        {
            return value;
        }
        
        protected bool GetIsMore()
        {
            return isMore;
        }

        //public virtual object Clone()
        //{
        //    return new OnPlanRule(GetId,)
        //}

    }

    public class ActionLimitRule : OnPlayRule
    {
        LimitType limitType;

        public ActionLimitRule(string id = "test-onclear", RuleTheme theme = RuleTheme.NORMAL, uint value = 0, bool isMore = false, LimitType limitType = LimitType.ALL) : base(id, theme, value, isMore) => this.limitType = limitType;

        public override string GetDescription()
        {
            return GetLimitTypeString() + (GetIsMore()?" is not less than ":" is not more than ")+ "<color=#F55135><b>"+GetValue()+"</b></color>";
        }

        public override bool CheckRule(StateValue currentState)
        {
            int currentValue = limitType switch
            {
                LimitType.ALL => currentState.ActionCount,
                LimitType.FORWARD => currentState.ForwardActionCount,
                LimitType.LEFT => currentState.LeftActionCount,
                LimitType.RIGHT => currentState.RightActionCount,
                LimitType.BACK => currentState.BackActionCount,
                LimitType.CONDITION => currentState.ConditionActionCount,
                _ => 0,
            };
            if (GetIsMore())
            {
                return currentValue >= GetValue();
            }
            else
            {
                return currentValue <= GetValue();
            }
        }

        public string GetLimitTypeString()
        {
            return limitType switch
            {
                LimitType.ALL => "Actions",
                LimitType.FORWARD => "Forward action(s)",
                LimitType.LEFT => "Left Forward action(s)",
                LimitType.RIGHT => "Right Forward action(s)",
                LimitType.BACK => "Back action(s)",
                LimitType.CONDITION => "Condition action(s)",
                _ => "[ ]",
            };
        }
    }

    public class ItemCollectorRule : OnPlayRule
    {
        public ItemCollectorRule(string id = "test-onclear", RuleTheme theme = RuleTheme.NORMAL, uint value = 0, bool isMore = false) : base(id, theme, value, isMore)
        {

        }

        public override string GetDescription()
        {
            return "[Do it Later]";
        }

        public override bool CheckRule(StateValue currentState)
        {
            return base.CheckRule(currentState);
        }
    }

}