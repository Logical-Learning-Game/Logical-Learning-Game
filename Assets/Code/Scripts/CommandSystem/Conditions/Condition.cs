using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Game.Conditions
{
    public enum ConditionName { MockTrue, MockFalse }
    public class Condition
    {
        private bool result = false;
        public ConditionName conditionName;

        public Condition(bool result = false)
        {
            this.result = result;
        }

        public void SetResult(bool result)
        {
            this.result = result;
        }

        public bool GetResult()
        {
            return result;
        }

        public void CheckCondition()
        {
            // mock ConditionPicker True/False
            if (conditionName == ConditionName.MockTrue)
            {
                SetResult(true);
            }
            else if (conditionName == ConditionName.MockFalse)
            {
                SetResult(false);
            }

        }

    }

}


