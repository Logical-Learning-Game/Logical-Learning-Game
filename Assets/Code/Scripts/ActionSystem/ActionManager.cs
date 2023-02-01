using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Unity.Game.Action
{
    public class ActionManager : MonoBehaviour
    {
        public static ActionManager Instance { get; private set; }
        public List<Action> actionList;

        void Awake()
        {
            if (Instance == null)
            {
                Debug.Log("actionManager Initialized");
                Instance = this;
                actionList = new List<Action>();
            }
            else
            {
                Destroy(gameObject);
            }
        }



        public IEnumerator AddAction(Action action)
        {
            actionList.Add(action);
            ActionBarManager.Instance.AddToContent(action);
            yield return action.Execute();
            // Action Execute Completed

        }



        public void ClearAction()
        {
            //ClearSequenceText();
            actionList.Clear();
            ActionBarManager.Instance.ClearContents();
        }
    }

}