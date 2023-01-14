using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Game.Conditions
{
    public enum ConditionSign { EMPTY,A, B, C, D, E }

    public class Condition
    {
        public ConditionSign sign;

        public Condition()
        {
            sign = ConditionSign.A;
        }

        public Condition(ConditionSign sign)
        {
            this.sign = sign;
        }
        public void SetConditionSign(ConditionSign sign)
        {
            this.sign = sign;
        }

        public bool CompareSign(ConditionSign tileSign)
        {
            if (tileSign == sign)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }

}


