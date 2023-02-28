using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalConfig;
using System;
using Newtonsoft.Json;

namespace Unity.Game.RuleSystem
{
    public enum RuleTheme { NORMAL, CONDITIONAL, LOOP }

    public enum LimitType { ALL,FORWARD,LEFT,RIGHT,BACK,CONDITION}


    [Serializable]
    public class Rule
    {

        [JsonProperty("map_rule_id")] public long Id { get; }
        [JsonProperty("rule_name")] public string RuleName { get; }
        [JsonProperty("rule_theme")] public RuleTheme Theme { get; }

        public Rule(long id = 0, string name = "rule-default", RuleTheme theme = RuleTheme.NORMAL)
        {
            Id = id;
            RuleName = name;
            Theme = theme;
        }
        
        public virtual string GetDescription()
        {
            return "Base Class Description";
        }
        
        public virtual bool CheckRule(StateValue currentState)
        {
            return false;
        }
    }
}