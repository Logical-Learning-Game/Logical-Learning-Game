using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Game.Conditions;
namespace Unity.Game.Command
{
    public class LinkerCommand : AbstractCommand
    {
        [SerializeField] private GameObject conditionSign;
        public override IEnumerator AddAction()
        {
            yield return null;
        }

        public void SetConditionSign(ConditionSign sign)
        {
            conditionSign.GetComponent<ConditionChoice>().SetCondition(sign);
        }

    }
}
