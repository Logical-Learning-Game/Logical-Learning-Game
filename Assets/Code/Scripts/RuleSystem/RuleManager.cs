using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalConfig;
using Unity.Game.Level;
using TMPro;


namespace Unity.Game.RuleSystem
{
    class RuleManager: MonoBehaviour
    {

        public static RuleManager Instance { get; private set; }

        private List<Rule> Rules;
        [SerializeField] private List<GameObject> RuleObjects;
        [SerializeField] private Sprite RuleComplete;
        [SerializeField] private Sprite RuleIncomplete;

        [SerializeField] private bool[] RuleStatus;
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
        }

    }
    
}