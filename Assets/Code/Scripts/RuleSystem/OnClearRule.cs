using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalConfig;
using System;

namespace Unity.Game.RuleSystem
{

    public class OnClearRule : Rule
    {
        public OnClearRule(string id = "test-onclear", RuleTheme theme = RuleTheme.NORMAL) : base(id, theme)
        {

        }
        public override string GetDescription()
        {
            return base.GetDescription();
        }
    }

    public class LevelClearRule : OnClearRule
    {
        public LevelClearRule(string id = "test-levelclear", RuleTheme theme = RuleTheme.NORMAL) : base(id, theme)
        {

        }

        public override string GetDescription()
        {
            Debug.Log("hey there, onclearrule is getting description");
            return "Reach the <color=#F5C500><b>GOAL</b></color>";
        }

        public override bool CheckRule()
        {
            return base.CheckRule();
        }
    }

}