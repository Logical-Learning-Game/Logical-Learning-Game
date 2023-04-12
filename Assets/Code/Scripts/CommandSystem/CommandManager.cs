using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using GlobalConfig;
using Unity.Game.MapSystem;
using Unity.Game.SaveSystem;
using Unity.Game.RuleSystem;
using Unity.Game.Level;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Unity.Game.Command
{
    public struct SubmitContext
    {
        public List<GameObject> Commands { get; set; }
        public Medal CommandMedal { get; set; }
        public Medal ActionMedal { get; set; }
        public StateValue StateValue { get; set; }
        public List<Rule> Rules { get; set; }
        public bool[] RuleStatus { get; set; }
        public bool IsFinited { get; set; }
        public bool IsCompleted { get; set; }
    }

    public enum CommandType 
    {
        [EnumMember(Value = "start")]
        START,
        [EnumMember(Value = "forward")]
        FORWARD,
        [EnumMember(Value = "left")]
        LEFT,
        [EnumMember(Value = "right")]
        RIGHT,
        [EnumMember(Value = "back")]
        BACK,
        [EnumMember(Value = "conditional_a")]
        CONDITIONAL_A,
        [EnumMember(Value = "conditional_b")]
        CONDITIONAL_B,
        [EnumMember(Value = "conditional_c")]
        CONDITIONAL_C,
        [EnumMember(Value = "conditional_d")]
        CONDITIONAL_D,
        [EnumMember(Value = "conditional_e")]
        CONDITIONAL_E 
    }

    public enum EdgeType 
    {
        [EnumMember(Value = "main_branch")]
        MAIN,
        [EnumMember(Value = "conditional_branch")]
        CONDITIONAL 
    }

    public class CommandManager : MonoBehaviour
    {
        public static CommandManager Instance { get; private set; }

        public static List<CommandState> savedCommandStates;
        public static event Action<SubmitContext> OnCommandSubmit;
        public static event Action OnCommandUpdate;

        public List<GameObject> commands;

        public GameObject selectedCommand;

        private int maxSteps = CommandConfig.COMMAND_MAX_STEP;
        private int currentSteps = 0;

        public bool isExecuting = false;
        private bool stopOnNextAction = false;
        public bool isFinited = false;
        //private bool isFreezing = false;
        public IEnumerator ExecuteIEnumerator;
        // Start is called before the first frame update
        public void InitCommands()
        {
            if (isExecuting)
            {
                SetIsExecuting(false);
            }
            //stopOnNextAction = false;

        }
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                commands = new List<GameObject>();
                savedCommandStates = new List<CommandState>();
                SaveCommandState();
            }
            else
            {
                Destroy(gameObject);
            }

        }

        // Update is called once per frame
        void Update()
        {
            //if (Input.GetKeyDown(KeyCode.Z))
            //{
            //    Debug.Log("Undoing");
            //    UndoCommand();
            //    CommandBarManager.Instance.OnUpdateCommandBar();
            //}

            if (Input.GetKeyDown(KeyCode.E))
            {
                ToggleExecute();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                //Debug.Log("Clearing");
                ClearCommands();
            }


            // no selectable right now
            //if (Input.GetKeyDown(KeyCode.Delete))
            //{
            //    if (selectedCommand && !isExecuting)
            //    {
            //        Debug.Log("Deleting " + selectedCommand.name);
            //        selectedCommand.GetComponent<AbstractCommand>().Delete();
            //    }
            //    CommandBarManager.Instance.OnUpdateCommandBar();
            //}

        }
        public void AddCommand(GameObject command)
        {
            commands.Add(command);
        }

        public static void SaveCommandState()
        {
            CommandState state = CommandState.GetCurrentState();

            //Debug.Log(state.commandSnapshots);
            //Debug.Log(savedCommandStates.LastOrDefault()?.commandSnapshots);
            //Debug.Log("state:" + (savedCommandStates.LastOrDefault()?.commandSnapshots != state.commandSnapshots));

            if (!CommandState.IsSameWithLastState(state, savedCommandStates))
            {
                savedCommandStates.Add(state);
                //Debug.Log("Saved");
            }

        }

        public static void UndoCommand()
        {
            if (savedCommandStates.Count > 1)
            {
                savedCommandStates[savedCommandStates.Count - 2].LoadCommandState();
                savedCommandStates.RemoveAt(savedCommandStates.Count - 1);
            }
        }

        public void ExecuteCommands()
        {

            OnCommandUpdate?.Invoke();
            if (!isExecuting)
            {
                if (VerifyCommand())
                {
                    //SetSelectedCommand(null);
                    ResetSteps();
                    SetIsExecuting(true);
                    isFinited = false;
                    GameObject startCommand = GameObject.FindGameObjectWithTag("StartCommand");
                    startCommand.GetComponent<AbstractCommand>().StartExecute();
                    StartCoroutine(MapViewManager.Instance.ViewPlayerMove());
                    AudioManager.PlayCommandStartSound();
                }
                else
                {
                    //Debug.Log("Invalid Command, Please Check");
                    AudioManager.PlayDefaultWarningSound();
                }
            }
        }

        public void ClearCommands() {

            if (!isExecuting)
            {
                MapManager.Instance.IsAuraInit = false;
                OnCommandUpdate?.Invoke();
                while (commands.Count > 0)
                {
                    commands[0].GetComponent<AbstractCommand>().Delete();
                }
                //foreach (GameObject command in commands)
                //{
                //    command.GetComponent<AbstractCommand>().Delete();
                //}
                commands.Clear();
                CommandBarManager.Instance.OnUpdateCommandBar();
            }
        }
        public void StopExecute()
        {
            SetStopOnNextAction(true);
        }

        public void ToggleExecute()
        {
            if (!isExecuting)
            {
                //Debug.Log("Executing");
                ExecuteCommands();
            }
            else
            {
                //Debug.Log("Stopping");
                StopExecute();
                AudioManager.PlayDefaultWarningSound();
            }
        }
        
        //temporatory remove selectable due to unexpected bugs

        //public void SetSelectedCommand(GameObject commandObject = null)
        //{
        //    //if (selectedCommand)
        //    //{
        //    //    selectedCommand.GetComponent<CommandStatus>().SetStatus(CommandStatus.Status.Default);
        //    //}

        //    selectedCommand = commandObject;

        //    if (selectedCommand)
        //    {
        //        selectedCommand.GetComponent<CommandStatus>().SetStatus(CommandStatus.Status.Selected);
        //    }

        //}

        public void StepUp()
        {
            currentSteps += 1;
        }

        private void ResetSteps()
        {
            currentSteps = 0;
        }

        public int GetRemainingStep()
        {
            return maxSteps - currentSteps;
        }

        public bool VerifyCommand()
        {
            // need more implementation
            //Debug.Log("Verifying");

            // Set All Command Status to Status.Error First
            foreach (AbstractCommand commandOnBoard in GetComponentsInChildren<AbstractCommand>())
            {
                //Debug.Log("Setting " + commandOnBoard.name + " to Error");
                commandOnBoard.status.SetStatus(CommandStatus.Status.Error);
            }

            GameObject startCommandObj = GameObject.FindGameObjectWithTag("StartCommand");
            if (startCommandObj == null)
            {
                //Debug.Log("No Start Command");
                return false;
            }

            List<AbstractCommand> verifyQueue = new List<AbstractCommand>();
            HashSet<AbstractCommand> verifiedSet = new HashSet<AbstractCommand>();

            verifyQueue.Add(startCommandObj.GetComponent<AbstractCommand>());

            // use bfs to verifying all reachable commands
            while (verifyQueue.Count > 0)
            {
                AbstractCommand verifyingCommand = verifyQueue[0];
                verifiedSet.Add(verifyingCommand);

                verifyingCommand.OnVerify();

                foreach (AbstractCommand commandNode in verifyingCommand.GetAllNextCommands())
                {
                    if (!verifiedSet.Contains(commandNode))
                    {
                        verifyQueue.Add(commandNode);
                    }
                }
                verifyQueue.RemoveAt(0);
            }

            // check if there are commands with Status.Error
            foreach (CommandStatus status in GetComponents<CommandStatus>())
            {
                if (status.GetStatus() == CommandStatus.Status.Error)
                {
                    return false;
                }
            }

            //Debug.Log("Verifying Completed");
            return true;
        }

        public void SetMaxSteps(int step)
        {
            // max step can be change in each level
            maxSteps = step;
        }

        public void OnExecute(AbstractCommand command)
        {
            // update linerenderer color when command is execute
            if (!isExecuting)
            {
                //Debug.Log("Not Executing Now, return");
                return;
            }

            if (stopOnNextAction == true)
            {
                //isFinited = false;
                //Debug.Log("CheckStopOnNextAction");
                SetStopOnNextAction(false);
                SetIsExecuting(false);
                OnSubmitFinish();
                return;
            }

            // check for limiting steps
            StepUp();
            if (GetRemainingStep() > 0)
            {
                if (command.nextCommand)
                {
                    command.nextCommand.StartExecute();
                }
                else
                {
                    isFinited = true;
                    SetIsExecuting(false);
                    OnSubmitFinish();
                    //Debug.Log("All Commands Executed successfully");
                }
            }
            else
            {
                isFinited = false;
                SetIsExecuting(false);
                OnSubmitFinish();
                //Debug.Log($"Max Step Exceed ({CommandConfig.COMMAND_MAX_STEP})");
            }
            
        }

        private void OnSubmitFinish()
        {
            (Medal commandMedal, Medal actionMedal) = (Medal.NONE, Medal.NONE);
            
            if(RuleManager.Instance.RuleStatus.All(x => x))
            {
                (commandMedal, actionMedal) = SubmitHistory.GetMedal((StateValue)RuleManager.Instance.CurrentStateValue.Clone(), LevelManager.Instance.GetMap());
            }

            var submitContext = new SubmitContext
            {
                Commands = commands,
                CommandMedal = commandMedal,
                ActionMedal = actionMedal,
                StateValue = (StateValue)RuleManager.Instance.CurrentStateValue.Clone(),
                Rules = LevelManager.Instance.GetRule(),
                RuleStatus = RuleManager.Instance.RuleStatus,
                IsFinited = isFinited,
                IsCompleted = LevelManager.Instance.GetIsPlayerReachGoal()
            };

            OnCommandSubmit?.Invoke(submitContext);
        }

        public void SetIsExecuting(bool isExecuting)
        {
            this.isExecuting = isExecuting;
            if(isExecuting == false)
            {
                StopCoroutine(ExecuteIEnumerator);
                //Debug.Log("Execute Coroutine is Stopped");
            }
        }

        public void SetStopOnNextAction(bool stopOnNextAction)
        {
            this.stopOnNextAction = stopOnNextAction;
        }

    }
}