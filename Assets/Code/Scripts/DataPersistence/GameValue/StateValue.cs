using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Game.Command;
using Unity.Game.ActionSystem;

namespace Unity.Game
{
    [System.Serializable]
    public class StateValue
    {
        public int CommandCount;
        public int ForwardCommandCount;
        public int LeftCommandCount;
        public int RightCommandCount;
        public int BackCommandCount;
        public int ConditionCommandCount;
        public int ActionCount;
        public int ForwardActionCount;
        public int LeftActionCount;
        public int RightActionCount;
        public int BackActionCount;
        public int ConditionActionCount;

        public StateValue()
        {
            CommandCount = 0;
            ForwardCommandCount = 0;
            LeftCommandCount = 0;
            RightCommandCount = 0;
            BackCommandCount = 0;
            ConditionCommandCount = 0;
            ActionCount = 0;
            ForwardActionCount = 0;
            LeftActionCount = 0;
            RightActionCount = 0;
            BackActionCount = 0;
            ConditionActionCount = 0;
        }

        public void UpdateCommandValue()
        {
            // update value of each command by finding GameObject
            int startCommandCount = GameObject.FindGameObjectsWithTag("StartCommand").Length;
            
            int forwardCommandCount = 0, leftCommandCount = 0, rightCommandCount = 0, backCommandCount = 0;
            foreach (GameObject command in GameObject.FindGameObjectsWithTag("MoveCommand"))
            {
                if(command.GetComponent<AbstractCommand>() is ForwardCommand)
                {
                    forwardCommandCount++;
                }
                if (command.GetComponent<AbstractCommand>() is LeftForwardCommand)
                {
                    leftCommandCount++;
                }
                if (command.GetComponent<AbstractCommand>() is RightForwardCommand)
                {
                    rightCommandCount++;
                }
                if (command.GetComponent<AbstractCommand>() is BackCommand)
                {
                    backCommandCount++;
                }
            }
            ForwardCommandCount = forwardCommandCount;
            LeftCommandCount = leftCommandCount;
            RightCommandCount = rightCommandCount;
            BackCommandCount = backCommandCount;
            ConditionCommandCount = GameObject.FindGameObjectsWithTag("ConditionCommand").Length;
            
            CommandCount = startCommandCount + ForwardCommandCount + LeftCommandCount + RightCommandCount + BackCommandCount + ConditionCommandCount;
        }

        public void UpdateActionValue()
        {
            int startActionCount = 0;
            int forwardActionCount = 0;
            int leftActionCount = 0;
            int rightActionCount = 0;
            int backActionCount = 0;
            int conditionActionCount = 0;

            foreach (Action action in ActionManager.Instance.actionList)
            {
                if (action is ForwardAction)
                {
                    forwardActionCount++;
                }
                else if (action is LeftForwardAction)
                {
                    leftActionCount++;
                }
                else if (action is RightForwardAction)
                {
                    rightActionCount++;
                }
                else if (action is BackAction)
                {
                    backActionCount++;
                }
                else if (action is ConditionAction)
                {
                    conditionActionCount++;
                }else if (action is StartAction)
                {
                    startActionCount++;
                }
            }

            ForwardActionCount = forwardActionCount;
            LeftActionCount = leftActionCount;
            RightActionCount = rightActionCount;
            BackActionCount = backActionCount;
            ConditionActionCount = conditionActionCount;
            ActionCount = startActionCount + forwardActionCount + leftActionCount + rightActionCount + backActionCount + conditionActionCount;
        }
    }

}