using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalConfig;
using System;
using Unity.Game;

namespace Unity.Game.RuleSystem
{

    public enum RuleTheme { NORMAL, CONDITIONAL, LOOP }

    public enum LimitType { ALL,FORWARD,LEFT,RIGHT,BACK,CONDITION}
    [System.Serializable]
    public class Rule /*: ICloneable*/
    {
        string id;
        RuleTheme theme;
        public Rule(string id = "", RuleTheme theme = RuleTheme.NORMAL)
        {
            this.id = id;
            this.theme = theme;
        }
        
        public virtual string GetDescription()
        {
            return "Base Class Description";
        }
        
        public virtual bool CheckRule(StateValue currentState)
        {
            return false;
        }

        public string GetId()
        {
            return id;
        }
        public RuleTheme GetTheme()
        {
            return theme;
        }
    }


}