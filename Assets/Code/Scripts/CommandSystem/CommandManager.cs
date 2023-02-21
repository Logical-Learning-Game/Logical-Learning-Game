using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalConfig;
using Unity.Game.MapSystem;
using Unity.Game.SaveSystem;
using Unity.Game.RuleSystem;
using Unity.Game.Level;

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

    public enum CommandType { START, FORWARD, LEFT, RIGHT, BACK, CONDITION_A, CONDITION_B, CONDITION_C, CONDITION_D, CONDITION_E }
    public enum EdgeType { NORMAL, CONDITIONAL }
    public class CommandManager : MonoBehaviour
    {
        public static CommandManager Instance { get; private set; }

        public static List<CommandState> savedCommandStates;
        public static event Action<SubmitContext> OnCommandSubmit;

        public List<GameObject> commands;

        public GameObject selectedCommand;

        private int maxSteps = CommandConfig.COMMAND_MAX_STEP;
        private int currentSteps = 0;

        public bool isExecuting = false;
        private bool stopOnNextAction = false;
        private bool isFreezing = false;


        // Start is called before the first frame update

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
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Debug.Log("Undoing");
                UndoCommand();
                CommandBarManager.Instance.OnUpdateCommandBar();
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                ToggleExecute();
            }

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                Debug.Log("Clearing");
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
                Debug.Log("Saved");
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
            //need more implementation
            if (!isExecuting)
            {
                if (VerifyCommand())
                {
                    //SetSelectedCommand(null);
                    ResetSteps();
                    SetIsExecuting(true);
                    GameObject startCommand = GameObject.FindGameObjectWithTag("StartCommand");
                    startCommand.GetComponent<AbstractCommand>().StartExecute();
                    StartCoroutine(MapViewManager.Instance.ViewPlayerMove());
                }
                else
                {
                    Debug.Log("Invalid Command, Please Check");
                }
            }
        }

        public void ClearCommands() {

            if (!isExecuting)
            {
                foreach (GameObject command in commands)
                {
                    command.GetComponent<AbstractCommand>().Delete();
                }
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
                Debug.Log("Executing");
                ExecuteCommands();
            }
            else
            {
                Debug.Log("Stopping");
                StopExecute();
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
            Debug.Log("Verifying");

            // Set All Command Status to Status.Error First
            foreach (AbstractCommand commandOnBoard in GetComponentsInChildren<AbstractCommand>())
            {
                //Debug.Log("Setting " + commandOnBoard.name + " to Error");
                commandOnBoard.status.SetStatus(CommandStatus.Status.Error);
            }

            GameObject startCommandObj = GameObject.FindGameObjectWithTag("StartCommand");
            if (startCommandObj == null)
            {
                Debug.Log("No Start Command");
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

            Debug.Log("Verifying Completed");
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

            if (stopOnNextAction == true)
            {
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
                    SetIsExecuting(false);
                    OnSubmitFinish();
                    Debug.Log("All Commands Executed");
                }
            }
            else
            {
                SetIsExecuting(false);
                OnSubmitFinish();
                Debug.Log("Max Step Exceed");
            }

        }

        private void OnSubmitFinish()
        {
            var submitContext = new SubmitContext
            {
                Commands = commands,
                CommandMedal = Medal.BRONZE,
                ActionMedal = Medal.SILVER,
                StateValue = RuleManager.Instance.CurrentStateValue,
                Rules = LevelManager.Instance.GetRule(),
                RuleStatus = RuleManager.Instance.RuleStatus,
                IsFinited = false,
                IsCompleted = true
            };

            OnCommandSubmit?.Invoke(submitContext);
        }

        public void SetIsExecuting(bool isExecuting)
        {
            this.isExecuting = isExecuting;
        }

        public void SetStopOnNextAction(bool stopOnNextAction)
        {
            this.stopOnNextAction = stopOnNextAction;
        }

    }
}