using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Game.Level;
using GlobalConfig;
using System;
using Unity.Game;
using Newtonsoft.Json;

namespace Unity.Game.RuleSystem
{

    public class OnClearRule : Rule
    {
        public OnClearRule(long id = 0, string name = "test-onclear", RuleTheme theme = RuleTheme.NORMAL) : base(id, name, theme)
        {

        }
        public override string GetDescription()
        {
            return base.GetDescription();
        }
    }

    public class LevelClearRule : OnClearRule
    {
        public LevelClearRule(long id = 0, string name = "test-levelclear", RuleTheme theme = RuleTheme.NORMAL) : base(id, name, theme)
        {

        }

        public override string GetDescription()
        {
            return "Reach the <b>GOAL</b>";
        }

        public override bool CheckRule(StateValue currentState)
        {
            return LevelManager.Instance.GetIsPlayerReachGoal();
        }
    }

}