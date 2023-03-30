using UnityEngine;
using Unity.Game.Command;
using Unity.Game.ActionSystem;
using Unity.Game.ItemSystem;
using Unity.Game.Level;
using System;
using System.Collections;
using Newtonsoft.Json;

namespace Unity.Game
{
    [Serializable]
    public class StateValue : ICloneable
    {
        [JsonProperty("command_count")] public int CommandCount;
        [JsonProperty("forward_command_count")] public int ForwardCommandCount;
        [JsonProperty("left_command_count")] public int LeftCommandCount;
        [JsonProperty("right_command_count")] public int RightCommandCount;
        [JsonProperty("back_command_count")] public int BackCommandCount;
        [JsonProperty("condition_command_count")] public int ConditionCommandCount;
        [JsonProperty("action_count")] public int ActionCount;
        [JsonProperty("forward_action_count")] public int ForwardActionCount;
        [JsonProperty("left_action_count")] public int LeftActionCount;
        [JsonProperty("right_action_count")] public int RightActionCount;
        [JsonProperty("back_action_count")] public int BackActionCount;
        [JsonProperty("condition_action_count")] public int ConditionActionCount;
        [JsonProperty("all_item_count")] public int AllItemCount;
        [JsonProperty("keya_item_count")] public int KeyACount;
        [JsonProperty("keyb_item_count")] public int KeyBCount;
        [JsonProperty("keyc_item_count")] public int KeyCCount;

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
                if (command.GetComponent<AbstractCommand>() is ForwardCommand)
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

            foreach (ActionSystem.Action action in ActionManager.Instance.actionList)
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
                }
                else if (action is StartAction)

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

        public void UpdateItemValue()
        {
            int keyA = 0, keyB = 0, keyC = 0;

            foreach (ItemType item in LevelManager.Instance.ItemList)
            {
                if (item == ItemType.KEY_A) keyA++;
                if (item == ItemType.KEY_B) keyB++;
                if (item == ItemType.KEY_C) keyC++;
            }

            KeyACount = keyA;
            KeyBCount = keyB;
            KeyCCount = keyC;
            AllItemCount = keyA + keyB + keyC;
        }

        public object Clone()
        {
            return new StateValue
            {
                CommandCount = CommandCount,
                ForwardCommandCount = ForwardCommandCount,
                LeftCommandCount = LeftCommandCount,
                RightCommandCount = RightCommandCount,
                BackCommandCount = BackCommandCount,
                ConditionCommandCount = ConditionCommandCount,
                ActionCount = ActionCount,
                ForwardActionCount = ForwardActionCount,
                LeftActionCount = LeftActionCount,
                RightActionCount = RightActionCount,
                BackActionCount = BackActionCount,
                ConditionActionCount = ConditionActionCount,
                AllItemCount = AllItemCount,
                KeyACount = KeyACount,
                KeyBCount = KeyBCount,
                KeyCCount = KeyCCount,
            };
        }

    }

}