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

    public class OnPlanRule : Rule
    {
        uint value;
        bool isMore;
        public OnPlanRule(long id = 0, string name = "test-onclear", RuleTheme theme = RuleTheme.NORMAL, uint value = 0, bool isMore = false) : base(id, name, theme)
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
    }

    public class CommandLimitRule : OnPlanRule
    {
        LimitType limitType;
        public CommandLimitRule(long id = 0, string name = "test-onclear", RuleTheme theme = RuleTheme.NORMAL, uint value = 0, bool isMore = false, LimitType limitType = LimitType.ALL) : base(id, name, theme, value, isMore) => this.limitType = limitType;

        public override string GetDescription()
        {
            return GetLimitTypeString() + (GetIsMore() ? " is not less than " : " is not more than ") + "<color=#F55135><b>" + GetValue() + "</b></color>";
        }

        public override bool CheckRule(StateValue currentState)
        {
            switch (limitType)
            {
                case LimitType.ALL:
                    return GetIsMore() ? (currentState.CommandCount >= GetValue()):(currentState.CommandCount <= GetValue());
                case LimitType.FORWARD:
                    return GetIsMore() ? (currentState.ForwardCommandCount >= GetValue()) : (currentState.ForwardCommandCount <= GetValue());
                case LimitType.LEFT:
                    return GetIsMore() ? (currentState.LeftCommandCount >= GetValue()) : (currentState.LeftCommandCount <= GetValue());
                case LimitType.RIGHT:
                    return GetIsMore() ? (currentState.RightCommandCount >= GetValue()) : (currentState.RightCommandCount <= GetValue());
                case LimitType.CONDITION:
                    return GetIsMore() ? (currentState.ConditionCommandCount >= GetValue()) : (currentState.ConditionCommandCount <= GetValue());
                default: return false;
            }
        }

        public string GetLimitTypeString()
        {
            return limitType switch
            {
                LimitType.ALL => "Command(s)",
                LimitType.FORWARD => "Forward command(s)",
                LimitType.LEFT => "Left Forward command(s)",
                LimitType.RIGHT => "Right Forward command(s)",
                LimitType.BACK => "Back command(s)",
                LimitType.CONDITION => "Condition command(s)",
                _ => "[ ]",
            };
        }
    }
}


