using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalConfig;
using Unity.Game.Level;
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

        [SerializeField] private bool[] RuleStatus;
        
        // there will be better implementation
        [SerializeField] private int[] RuleCurrentValue;
        
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
            //RuleCurrentValue = new int[3] { -1, -1, -1 };
        }

        public void OnPlanCheck()
        {
            for (int i = 0; i < Rules.Count; i++)
            {
                if (Rules[i] is OnPlanRule)
                {
                    bool result = Rules[i].CheckRule();
                    RuleObjects[i].GetComponentInChildren<SpriteRenderer>().sprite = result ? RuleComplete : RuleIncomplete;
                    RuleStatus[i] = result;
                }
            }
        }

        public void OnPlayCheck()
        {
            for (int i = 0; i < Rules.Count; i++)
            {
                if (Rules[i] is OnPlayRule)
                {
                    bool result = Rules[i].CheckRule();
                    RuleObjects[i].GetComponentInChildren<SpriteRenderer>().sprite = result ? RuleComplete : RuleIncomplete;
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
                    bool result = Rules[i].CheckRule();
                    RuleObjects[i].GetComponentInChildren<SpriteRenderer>().sprite = result ? RuleComplete : RuleIncomplete;
                    RuleStatus[i] = result;
                }
            }
        }

    }

}