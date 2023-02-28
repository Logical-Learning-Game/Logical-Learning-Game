using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalConfig;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Unity.Game.RuleSystem
{
    [Serializable]
    public class IRule
    {
        static readonly List<LimitType> LimitTypeIndex = new List<LimitType>() { LimitType.ALL, LimitType.FORWARD, LimitType.LEFT, LimitType.RIGHT, LimitType.BACK, LimitType.CONDITION };

        [JsonProperty("map_rule_id")] public long RuleId;
        [JsonProperty("rule_name")][JsonConverter(typeof(StringEnumConverter))] public RuleName RuleName;
        [JsonProperty("rule_theme")][JsonConverter(typeof(StringEnumConverter))] public RuleTheme RuleTheme;
        [JsonProperty("parameters")] public List<uint> Parameters;

        public Rule GetRule()
        {
            switch (RuleName)
            {
                case RuleName.ACTION_LIMIT_RULE:
                    return new ActionLimitRule(id: RuleId, theme: RuleTheme, value: Parameters[0], isMore: Parameters[1] == 1, limitType: LimitTypeIndex[(int)Parameters[2]]);
                case RuleName.COMMAND_LIMIT_RULE:
                    return new CommandLimitRule(id: RuleId, theme: RuleTheme, value: Parameters[0], isMore: Parameters[1] == 1, limitType: LimitTypeIndex[(int)Parameters[2]]);
                case RuleName.ITEM_COLLECTOR_RULE:
                    return new LevelClearRule(id: RuleId);
                case RuleName.LEVEL_CLEAR_RULE:
                    return new LevelClearRule(id: RuleId);
                default: return new LevelClearRule(id: RuleId);
            }
        }
    }

}