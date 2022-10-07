using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Unity.Game.Action
{
    public class ActionManager : MonoBehaviour
    {
        // Start is called before the first frame update
        public static ActionManager Instance { get; private set; }
        private TMP_Text actionSequence;
        public List<Action> actionList;

        void Awake()
        {
            if (Instance == null)
            {
                Debug.Log("actionManager Initialized");
                Instance = this;
                actionSequence = GameObject.Find("ActionSequence").GetComponent<TMP_Text>();
                actionList = new List<Action>();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void AddSequenceText(string text)
        {
            actionSequence.text += text;
        }

        public IEnumerator AddAction(Action action)
        {
            AddSequenceText(action.actionName + "\n");
            actionList.Add(action);

            yield return action.Execute();
            // Action Execute Completed
            
        }

        private void ClearSequenceText()
        {
            actionSequence.text = "Action:\n";
        }

        public void ClearAction()
        {
            ClearSequenceText();
            actionList.Clear();
        }
    }

}