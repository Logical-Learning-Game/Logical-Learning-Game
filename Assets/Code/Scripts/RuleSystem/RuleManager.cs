using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalConfig;
using Unity.Game.Level;
using UnityEngine.UI;
using TMPro;


namespace Unity.Game.RuleSystem
{
    class RuleManager : MonoBehaviour
    {
        public static RuleManager Instance { get; private set; }

        private List<Rule> Rules;
        [SerializeField] private List<GameObject> RuleObjects;
        [SerializeField] private Sprite RuleComplete;
        [SerializeField] private Sprite RuleIncomplete;

        [SerializeField] public bool[] RuleStatus;

        public StateValue CurrentStateValue;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void Start()
        {


        }

        public void InitRule()
        {
            Rules = LevelManager.Instance.GetRule();
            for (int i = 0; i < Rules.Count; i++)
            {
                RuleObjects[i].GetComponentInChildren<TMP_Text>().text = Rules[i].GetDescription();
            }
            RuleStatus = new bool[3] { false, false, false };
            CurrentStateValue = new StateValue();
            InitCheck();
        }

        public void InitCheck()
        {
            CurrentStateValue.UpdateCommandValue();
            CurrentStateValue.UpdateActionValue();
            for (int i = 0; i < Rules.Count; i++)
            {
                bool result = Rules[i].CheckRule(CurrentStateValue);
                RuleObjects[i].GetComponentInChildren<Image>().sprite = result ? RuleComplete : RuleIncomplete;
                RuleStatus[i] = result;
            }
        }
        public void OnPlanCheck()
        {
            CurrentStateValue.UpdateCommandValue();
            
            for (int i = 0; i < Rules.Count; i++)
            {
                
                if (Rules[i] is OnPlanRule)
                {
                    bool result = Rules[i].CheckRule(CurrentStateValue);
                    RuleObjects[i].GetComponentInChildren<Image>().sprite = result ? RuleComplete : RuleIncomplete;
                    RuleStatus[i] = result;
                }
            }
        }

        public void OnPlayCheck()
        {
            CurrentStateValue.UpdateActionValue();
            for (int i = 0; i < Rules.Count; i++)
            {
                if (Rules[i] is OnPlayRule)
                {
                    bool result = Rules[i].CheckRule(CurrentStateValue);
                    RuleObjects[i].GetComponentInChildren<Image>().sprite = result ? RuleComplete : RuleIncomplete;
                    RuleStatus[i] = result;
                }
            }
        }

        public void OnClearCheck()
        {
            for (int i = 0; i < Rules.Count; i++)
            {
                if (Rules[i] is OnClearRule)
                {
                    bool result = Rules[i].CheckRule(CurrentStateValue);
                    RuleObjects[i].GetComponentInChildren<Image>().sprite = result ? RuleComplete : RuleIncomplete;
                    RuleStatus[i] = result;
                }
            }
        }

    }

}