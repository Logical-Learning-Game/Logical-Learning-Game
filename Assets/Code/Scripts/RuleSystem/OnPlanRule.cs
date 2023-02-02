using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalConfig;
using System;
using Unity.Game.ActionSystem;
using Unity.Game;

namespace Unity.Game.RuleSystem
{

    public class OnPlanRule : Rule
    {
        uint value;
        bool isMore;
        public OnPlanRule(string id = "test-onclear", RuleTheme theme = RuleTheme.NORMAL, uint value = 0, bool isMore = false) : base(id, theme)
        {
            this.value = value;
            this.isMore = isMore;
        }
        public override string GetDescription()
        {
            return base.GetDescription();
        }
    }

    public class CommandLimitRule : OnPlayRule
    {
        LimitType limitType;
        public CommandLimitRule(string id = "test-onclear", RuleTheme theme = RuleTheme.NORMAL, uint value = 0, bool isMore = false, LimitType limitType = LimitType.ALL) : base(id, theme, value, isMore) => this.limitType = limitType;

        public override string GetDescription()
        { 
            return GetLimitTypeString() + (GetIsMore() ? " is not less than " : " is not more than ") + "<color=#F55135><b>" + GetValue() + "</b></color>";
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