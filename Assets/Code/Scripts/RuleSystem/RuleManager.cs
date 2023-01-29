using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalConfig;
using Unity.Game.Level;



namespace Unity.Game.RuleSystem
{
    class RuleManager: MonoBehaviour
    {

        public static RuleManager Instance { get; private set; }

        public List<Rule> Rules;

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
        }

    }
    
}