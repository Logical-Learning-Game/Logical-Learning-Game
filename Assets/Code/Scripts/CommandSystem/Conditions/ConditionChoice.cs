using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Unity.Game.Conditions
{
    public class ConditionChoice : MonoBehaviour
    {
        [SerializeField] private ConditionSign condition;
        [SerializeField] private GameObject conditionDisplay;

        public void SetCondition(ConditionSign sign)
        {
            condition = sign;
            if (sign != ConditionSign.EMPTY)
            {
                conditionDisplay.GetComponent<TMP_Text>().text = sign.ToString();
            }
            else
            {
                conditionDisplay.GetComponent<TMP_Text>().text = "";

            }
        }

        public ConditionSign GetCondition()
        {
            return condition;
        }

    }
}