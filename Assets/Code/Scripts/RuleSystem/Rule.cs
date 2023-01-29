using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalConfig;
using System;

namespace Unity.Game.RuleSystem
{
  
    public enum RuleTheme { NORMAL,CONDITIONAL,LOOP}
    public class Rule : ICloneable
    {
        string id;
        RuleTheme theme;
        string description;
        public Rule(string id = "", RuleTheme theme = RuleTheme.NORMAL, string description = "")
        {
            this.id = id;
            this.theme = theme;
            this.description = description;
        }
        public object Clone()
        {
            return new Rule(id, theme, description);
        }
    }

    class NormalClearRule : Rule
    {
        public NormalClearRule(string id = "", RuleTheme theme = RuleTheme.NORMAL, string description = "") : base(id, theme, description)
        {
        }
    }
    class CommandLimitRule : Rule
    {
        public CommandLimitRule(string id = "", RuleTheme theme = RuleTheme.NORMAL, string description = "") : base(id, theme, description)
        {
        }
    }

    class ActionLimitRule : Rule
    {
        public ActionLimitRule(string id = "", RuleTheme theme = RuleTheme.NORMAL, string description = "") : base(id, theme, description)
        {
        }
    }

}