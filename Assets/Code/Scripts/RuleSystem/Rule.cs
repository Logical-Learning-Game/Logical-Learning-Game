using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalConfig;
using System;

namespace Unity.Game.RuleSystem
{

    public enum RuleTheme { NORMAL, CONDITIONAL, LOOP }

    public enum LimitType { ALL,FORWARD,LEFT,RIGHT,BACK,CONDITION}
    public class Rule /*: ICloneable*/
    {
        string id;
        RuleTheme theme;
        public Rule(string id = "", RuleTheme theme = RuleTheme.NORMAL)
        {
            this.id = id;
            this.theme = theme;
        }
        //public virtual object Clone()
        //{
        //    return new Rule(id, theme);
        //}
        
        //public Rule GetRule()
        //{
        //    return (Rule)Clone();
        //}
        
        public virtual string GetDescription()
        {
            return "Base Class Description";
        }

        public virtual bool CheckRule()
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