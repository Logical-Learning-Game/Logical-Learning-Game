using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalConfig;
using System;
using Unity.Game.ActionSystem;
using Unity.Game;
using Newtonsoft.Json;


namespace Unity.Game.RuleSystem
{

    public class OnPlayRule : Rule
    {
        uint value;
        bool isMore;
        public OnPlayRule(long id = 0, string name = "test-onclear", RuleTheme theme = RuleTheme.NORMAL, uint value = 0, bool isMore = false) : base(id, name, theme)
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

        public ActionLimitRule(long id = 0, string name = "test-onclear", RuleTheme theme = RuleTheme.NORMAL, uint value = 0, bool isMore = false, LimitType limitType = LimitType.ALL) : base(id, name, theme, value, isMore) => this.limitType = limitType;

        public override string GetDescription()
        {
            return $"{GetLimitTypeString()} {(GetIsMore()?" is not less than ":" is not more than ")} <color=#E30B00><b>{GetValue()}</b></color> ";
        }

        public override bool CheckRule(StateValue currentState)
        {
            switch (limitType)
            {
                case LimitType.ALL:
                    return GetIsMore() ? (currentState.ActionCount >= GetValue()) : (currentState.ActionCount <= GetValue());
                case LimitType.FORWARD:
                    return GetIsMore() ? (currentState.ForwardActionCount >= GetValue()) : (currentState.ForwardActionCount <= GetValue());
                case LimitType.LEFT:
                    return GetIsMore() ? (currentState.LeftActionCount >= GetValue()) : (currentState.LeftActionCount <= GetValue());
                case LimitType.RIGHT:
                    return GetIsMore() ? (currentState.RightActionCount >= GetValue()) : (currentState.RightActionCount <= GetValue());
                case LimitType.CONDITION:
                    return GetIsMore() ? (currentState.ConditionActionCount >= GetValue()) : (currentState.ConditionActionCount <= GetValue());
                default: return false;
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
        public ItemCollectorRule(long id, string name = "test-onclear", RuleTheme theme = RuleTheme.NORMAL, uint value = 0, bool isMore = false) : base(id, name, theme, value, isMore)
        {

        }

        public override string GetDescription()
        {
            return $"Have {(GetIsMore() ? "not less than" : "not more than")} <color=#E30B00><b>{GetValue()}</b></color> item(s) in your inventory";
        }

        public override bool CheckRule(StateValue currentState)
        {
            // simple checking rule, if all item is more than
            //return currentState.AllItemCount == GetValue();
            return GetIsMore() ? (currentState.AllItemCount >= GetValue()) : (currentState.AllItemCount <= GetValue());
        }
    }

}