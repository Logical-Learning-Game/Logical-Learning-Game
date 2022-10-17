using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Game.Conditions
{


    public class Condition
    {
        private bool result = false;

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

    }
}