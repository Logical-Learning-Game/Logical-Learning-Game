using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Unity.Game.RuleSystem;


namespace Unity.Game.Command
{
    public class CommandBarManager : MonoBehaviour
    {
        public static CommandBarManager Instance { get; private set; }

        [SerializeField]
        private List<CommandInitiator> commandInitiators;

        [SerializeField]
        private CommandInitiator startCommandInitiator;
        [SerializeField]
        private CommandInitiator forwardCommandInitiator;
        [SerializeField]
        private CommandInitiator leftForwardCommandInitiator;
        [SerializeField]
        private CommandInitiator rightForwardCommandInitiator;
        [SerializeField]
        private CommandInitiator backCommandInitiator;
        [SerializeField]
        private CommandInitiator conditionCommandInitiator;


        private MaxCommand maxCommand;
        public MaxCommand MaxCommand
        {
            get { return maxCommand; }
            set { maxCommand = value; }
        }

        private HorizontalLayoutGroup layoutGroup;

        private float spacing;
        private void Awake()
        {
            if (Instance == null)
            {
                //Debug.Log("CMDBarMGR Awake");
                Instance = this;
                layoutGroup = GetComponentInChildren<HorizontalLayoutGroup>();
                commandInitiators = new List<CommandInitiator>()
                 {
                    startCommandInitiator,
                    forwardCommandInitiator,
                    leftForwardCommandInitiator,
                    rightForwardCommandInitiator,
                    backCommandInitiator,
                    conditionCommandInitiator,
                 };
                MaxCommand = new MaxCommand();
            }
            else
            {
                Destroy(gameObject);
            }

        }

        private float CalculateSpacing()
        {
            return 4f;
        }

        private void Update()
        {
            layoutGroup.spacing = CalculateSpacing();
        }

        public void DisplayConditionInitiator(bool isDisplay)
        {
            conditionCommandInitiator.gameObject.SetActive(isDisplay);
        }

        public void OnUpdateCommandBar()
        {
            RuleManager.Instance.OnPlanCheck();
            CheckRemainingCommand();
            foreach (CommandInitiator commandInitiator in commandInitiators)
            {
                commandInitiator.OnUpdateCommandBar();
            }
            
        }

        public void CheckRemainingCommand()
        {
            
            // need more implementation later
            if (GameObject.FindGameObjectsWithTag("StartCommand").Length >= maxCommand.Start)
            {
                GameObject.Find("StartCommandInitiator").GetComponent<CommandInitiator>().setEnabled(false);
            }
            else
            {
                GameObject.Find("StartCommandInitiator").GetComponent<CommandInitiator>().setEnabled(true);
            }
        }
    }
}
