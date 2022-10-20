using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Game.Action;
using Unity.Game.Conditions;

namespace Unity.Game.Command
{
    public class ConditionCommand : AbstractCommand
    {
        // default nextCommand is false case;
        public Condition commandCondition = new Condition();


        [SerializeField]
        private AbstractCommand linkerCommand;

        public override IEnumerator Execute()
        {
            Debug.Log("Condition Command Executing");
            status.SetStatus(CommandStatus.Status.Executing);
            linkerCommand.status.SetStatus(CommandStatus.Status.Executing);
            
            yield return AddAction();
            
            Debug.Log("Executing Complete");

            if (commandCondition.GetResult())
            {
                // lead to NextCommand of LinkerCommand When Result is True
                status.SetStatus(CommandStatus.Status.Default);
                linkerCommand.status.SetStatus(CommandStatus.Status.Success);
                CommandManager.Instance.OnExecute(linkerCommand);
            }
            else
            {
                
                // lead to Default NextCommand
                status.SetStatus(CommandStatus.Status.Success);
                linkerCommand.status.SetStatus(CommandStatus.Status.Default);
                CommandManager.Instance.OnExecute(this);
            }

        }

        public override IEnumerator AddAction()
        {
            yield return ActionManager.Instance.AddAction(new Action.ConditionAction(commandCondition));
            Debug.Log("Condition Result: " + commandCondition.GetResult());
        }

        protected override void Awake()
        {
            linkerCommand = GetComponentInChildren<LinkerCommand>();
            base.Awake();
            
        }

        public override void OnVerify()
        {
            base.OnVerify();
            linkerCommand.OnVerify();
        }

        public override List<AbstractCommand> GetAllNextCommands()
        {
            List<AbstractCommand> AllNextCommands = new List<AbstractCommand>() { };
            if (nextCommand != null)
            {
                AllNextCommands.Add(nextCommand);
            }
            if (linkerCommand!= null)
            {
                if( linkerCommand.nextCommand != null)
                {
                    AllNextCommands.Add(linkerCommand.nextCommand);
                } 
            }
            return AllNextCommands;
        }

        public void SetCondition(Condition condition)
        {
            Debug.Log("Setting Condition to: " + condition.conditionName);
            commandCondition = condition;
            OnSetCondition();
        }

        private void OnSetCondition()
        {
            linkerCommand.transform.Find("CommandSign").GetComponent<Image>().color = commandCondition.conditionName == ConditionName.MockTrue ? Color.green : Color.red;
        }
    }
    

}
