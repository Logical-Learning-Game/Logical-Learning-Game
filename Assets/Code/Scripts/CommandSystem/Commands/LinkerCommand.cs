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

        [SerializeField]
        public AbstractCommand conditionCommand;

        protected override void Awake()
        {
            conditionCommand = GetComponentInParent<ConditionCommand>();
            base.Awake();
        }
        public override IEnumerator AddAction()
        {
            yield return null;
        }

        public override void LinkTo(AbstractCommand nextCommand)
        {
            if (nextCommand == conditionCommand)
            {
                //Debug.Log("same command");
            }
            else
            {
                base.LinkTo(nextCommand);
            }

        }

        public void SetConditionSign(ConditionSign sign)
        {
            conditionSign.GetComponent<ConditionChoice>().SetCondition(sign);
        }

    }
}
