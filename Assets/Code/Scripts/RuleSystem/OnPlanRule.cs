using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalConfig;
using System;
using Unity.Game.ActionSystem;

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

        public override bool CheckRule()
        {
            return base.CheckRule();
        }

        public string GetLimitTypeString()
        {
            switch (limitType)
            {
                case LimitType.ALL:
                    return "Commands";
                case LimitType.FORWARD:
                    return "Forward command(s)";
                case LimitType.LEFT:
                    return "Left Forward command(s)";
                case LimitType.RIGHT:
                    return "Right Forward command(s)";
                case LimitType.BACK:
                    return "Back command(s)";
                case LimitType.CONDITION:
                    return "Condition command(s)";
                default: return "[ ]";
            }
        }
    }


}