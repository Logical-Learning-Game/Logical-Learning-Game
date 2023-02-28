using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalConfig;
using System;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Unity.Game.RuleSystem
{
    public enum RuleTheme
    {
        [EnumMember(Value = "normal")]
        NORMAL,
        [EnumMember(Value = "conditional")]
        CONDITIONAL,
        [EnumMember(Value = "loop")]
        LOOP
    }

    public enum RuleName
    {
        [EnumMember(Value = "action_limit_rule")]
        ACTION_LIMIT_RULE,
        [EnumMember(Value = "command_limit_rule")]
        COMMAND_LIMIT_RULE,
        [EnumMember(Value = "item_collector_rule")]
        ITEM_COLLECTOR_RULE,
        [EnumMember(Value = "level_clear_rule")]
        LEVEL_CLEAR_RULE
    }

    public enum LimitType { ALL, FORWARD, LEFT, RIGHT, BACK, CONDITION }


    [Serializable]
    public class Rule
    {

        public long Id { get; }
        public string RuleName { get; }
        public RuleTheme Theme { get; }

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