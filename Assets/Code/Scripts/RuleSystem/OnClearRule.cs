using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Game.Level;
using GlobalConfig;
using System;
using Unity.Game;

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
            return "Reach the <color=#F5C500><b>GOAL</b></color>";
        }

        public override bool CheckRule(StateValue currentState)
        {
            return LevelManager.Instance.GetIsPlayerReachGoal();
        }
    }

}