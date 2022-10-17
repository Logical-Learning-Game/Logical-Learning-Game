using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Game.Action;

namespace Unity.Game.Command
{
    public class ConditionCommand : AbstractCommand
    {
        // default nextCommand is false case;
        public Condition commandCondition = new Condition();


        [SerializeField]
        private AbstractCommand linkerCommand;
        
        public override IEnumerator AddAction()
        {
            yield return ActionManager.Instance.AddAction(new Action.ConditionAction(commandCondition));
            Debug.Log("conditionresult" + commandCondition.GetResult());
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
    }
    

}
