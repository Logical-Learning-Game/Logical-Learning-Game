using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalConfig;
using System;

namespace Unity.Game.RuleSystem
{

    public enum RuleTheme { NORMAL, CONDITIONAL, LOOP }
    public class Rule : ICloneable
    {
        string id;
        RuleTheme theme;
        string description;
        public Rule(string id = "", string description = "", RuleTheme theme = RuleTheme.NORMAL)
        {
            this.id = id;
            this.theme = theme;
            this.description = description;
        }
        public object Clone()
        {
            return new Rule(id, description, theme);
        }
        
        public virtual string GetDescription()
        {
            return description;
        }
    }

    class NormalClearRule : Rule
    {
        public NormalClearRule(string id = "test-normal", string description = "Reach the <color=#F5C500><b>GOAL</b></color>", RuleTheme theme = RuleTheme.NORMAL) : base(id, description, theme)
        {
        }
    }
    class CommandLimitRule : Rule
    {
        public CommandLimitRule(string id = "test-commandlimit", string description = "Command(s) not more than <color=#F55135><b>x</b></color>", RuleTheme theme = RuleTheme.NORMAL) : base(id, description, theme)
        {
        }
    }

    class ActionLimitRule : Rule
    {
        public ActionLimitRule(string id = "test-actionlimt", string description = "Action(s) not more than  <color=#F55135><b>x</b></color>", RuleTheme theme = RuleTheme.NORMAL) : base(id, description, theme)
        {
        }
    }

}