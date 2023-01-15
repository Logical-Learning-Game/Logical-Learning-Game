using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Game.Action;
using Unity.Game.Conditions;
using Unity.Game.MapSystem;

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
            ConditionSign tileSign = Player.Instance.GetLastSign();

            if (commandCondition.CompareSign(tileSign))
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

        public void SetCondition(ConditionSign condition)
        {
            Debug.Log("Setting Condition to: " + condition.ToString());
            commandCondition.SetConditionSign(condition);
            OnSetCondition();
        }

        private void OnSetCondition()
        {
            linkerCommand.GetComponent<LinkerCommand>().SetConditionSign(commandCondition.sign);

            //linkerCommand.transform.Find("CommandSign").GetComponent<Image>().color = commandCondition.sign == ConditionName.MockTrue ? Color.green : Color.red;
        }
    }
    

}
