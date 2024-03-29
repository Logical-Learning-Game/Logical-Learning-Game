using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Game.ActionSystem;
using Unity.Game.Conditions;
using Unity.Game.Level;

namespace Unity.Game.Command
{
    public class ConditionCommand : AbstractCommand
    {
        // default nextCommand is false case;
        public Condition commandCondition = new Condition();

        [SerializeField]
        public AbstractCommand linkerCommand;

        public override IEnumerator Execute()
        {
            //Debug.Log("Condition Command Executing");
            status.SetStatus(CommandStatus.Status.Executing);
            linkerCommand.status.SetStatus(CommandStatus.Status.Executing);
            
            yield return AddAction();
            
            //Debug.Log("Executing Complete");
            ConditionSign tileSign = LevelManager.Instance.GetLastSign();

            if (commandCondition.CompareSign(tileSign))
            {
                // lead to NextCommand of LinkerCommand When Result is True
                status.SetStatus(CommandStatus.Status.Default);
                linkerCommand.status.SetStatus(CommandStatus.Status.Success);
                CommandManager.Instance.OnExecute(linkerCommand);

                // Consume Player Current Condition
                LevelManager.Instance.SetLastSign(ConditionSign.EMPTY);
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
            yield return ActionManager.Instance.AddAction(new ConditionAction(commandCondition));
        }

        protected override void Awake()
        {
            linkerCommand = GetComponentInChildren<LinkerCommand>();
            base.Awake();
            SetCondition(ConditionSign.A);
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
            //Debug.Log("Setting Condition to: " + condition.ToString());
            commandCondition.SetConditionSign(condition);
            OnSetCondition();

            switch (condition)
            {
                case ConditionSign.A:
                    SetCommandType(CommandType.CONDITIONAL_A);
                    break;
                case ConditionSign.B:
                    SetCommandType(CommandType.CONDITIONAL_B);
                    break;
                case ConditionSign.C:
                    SetCommandType(CommandType.CONDITIONAL_C);
                    break;
                case ConditionSign.D:
                    SetCommandType(CommandType.CONDITIONAL_D);
                    break;
                case ConditionSign.E:
                    SetCommandType(CommandType.CONDITIONAL_E);
                    break;
            }
            
        }

        private void OnSetCondition()
        {
            linkerCommand.GetComponent<LinkerCommand>().SetConditionSign(commandCondition.sign);

            //linkerCommand.transform.Find("CommandSign").GetComponent<Image>().color = commandCondition.sign == ConditionName.MockTrue ? Color.green : Color.red;
        }

        public override void Delete()
        {
            base.Delete();
            linkerCommand.Delete();
        }
    }
    

}
