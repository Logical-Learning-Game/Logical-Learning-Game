using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Game.Conditions
{
    public class ConditionChoice : MonoBehaviour
    {
        [SerializeField]
        private Condition condition = new Condition();
        public Condition Condition
        {
            get { return condition; }
            set { condition = value; }
        }

        
    }
}