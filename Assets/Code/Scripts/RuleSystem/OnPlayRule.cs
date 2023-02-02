using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalConfig;
using System;
using Unity.Game.ActionSystem;

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

        public override bool CheckRule()
        {
            return base.CheckRule();
        }

        public string GetLimitTypeString()
        {
            switch (limitType)
            {
                case LimitType.ALL:
                    return "Actions";
                case LimitType.FORWARD:
                    return "Forward action(s)";
                case LimitType.LEFT:
                    return "Left Forward action(s)";
                case LimitType.RIGHT:
                    return "Right Forward action(s)";
                case LimitType.BACK:
                    return "Back action(s)";
                case LimitType.CONDITION:
                    return "Condition action(s)";
                default: return "[ ]";
            }
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

        public override bool CheckRule()
        {
            return base.CheckRule();
        }
    }

}