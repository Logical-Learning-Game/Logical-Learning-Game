using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace Unity.Game.Command
{
    public class CommandManager : MonoBehaviour
    {
        public static CommandManager Instance { get; private set; }

        public static List<CommandState> savedCommandStates;

        public List<GameObject> commands;

        public GameObject selectedCommand;

        private int maxSteps = 10;
        private int currentSteps = 0;

        

        public bool isExecuting = false;

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

            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Executing");
                ExecuteCommands();
            }

            if (Input.GetKeyDown(KeyCode.Delete))
            {
                if (selectedCommand && !isExecuting)
                {
                    Debug.Log("Deleting " + selectedCommand.name);
                    selectedCommand.GetComponent<AbstractCommand>().Delete();
                }
                CommandBarManager.Instance.OnUpdateCommandBar();
            }

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
                    SetSelectedCommand(null);
                    ResetSteps();
                    SetisExecuting(true);
                    GameObject startCommand = GameObject.FindGameObjectWithTag("StartCommand");
                    startCommand.GetComponent<AbstractCommand>().StartExecute();
                }
                else
                {
                    Debug.Log("Invalid Command, Please Check");
                }
            }


        }

        public void SetSelectedCommand(GameObject commandObject = null)
        {
            if (selectedCommand)
            {
                selectedCommand.GetComponent<CommandStatus>().SetStatus(CommandStatus.Status.Default);
            }

            selectedCommand = commandObject;

            if (selectedCommand)
            {
                selectedCommand.GetComponent<CommandStatus>().SetStatus(CommandStatus.Status.Selected);
            }

        }

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
            // need implementation
            Debug.Log("Verifying");

            foreach (GameObject commandObj in commands)
            {
                commandObj.GetComponent<AbstractCommand>().status.SetStatus(CommandStatus.Status.Error);
            }

            GameObject startCommandObj = GameObject.FindGameObjectWithTag("StartCommand");
            if (startCommandObj == null)
            {
                return false;
            }

            AbstractCommand command = startCommandObj.GetComponent<AbstractCommand>();
            HashSet<AbstractCommand> verifiedSet = new HashSet<AbstractCommand>();
            while (command && !verifiedSet.Contains(command))
            {
                verifiedSet.Add(command);
                command.status.SetStatus(CommandStatus.Status.Default);
                command = command.GetNextCommand();
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
                    SetisExecuting(false);
                    Debug.Log("All Commands Executed");
                }
            }
            else
            {
                SetisExecuting(false);
                Debug.Log("Max Step Exceed");
            }

        }

        public void SetisExecuting(bool isExecuting)
        {
            this.isExecuting = isExecuting;
        }

    }
}