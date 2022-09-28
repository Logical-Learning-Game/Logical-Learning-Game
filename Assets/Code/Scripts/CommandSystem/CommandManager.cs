using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class CommandManager : MonoBehaviour
{
    public static CommandManager Instance { get; private set; }

    public static List<CommandState> savedCommandStates;

    public List<GameObject> commands;

    public GameObject selectedCommand;

    private int maxSteps = 10;
    private int currentSteps = 0;

    private MaxCommand maxCommand;


    // Start is called before the first frame update

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            commands = new List<GameObject>();
            savedCommandStates = new List<CommandState>();
            maxCommand = new MaxCommand();
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
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Executing");
            ExecuteCommands();
        }

        OnCheckRemainingCommand();
        OnSelectCommand();

        //
    }
    public void AddCommand(GameObject command)
    {
        commands.Add(command);
    }

    public void OnSelectCommand()
    {
        foreach (GameObject command in commands)
        {
            if (selectedCommand == command)
            {
                command.GetComponentInChildren<CommandSelectable>().SetisSelected(true);
                //command.GetComponent<Draggable>().SetisDraggable(false);
            }
            else
            {
                command.GetComponentInChildren<CommandSelectable>().SetisSelected(false);
                //command.GetComponent<Draggable>().SetisDraggable(true);
            }
        }

    }
    private int GetCommandCount()
    {
        return GameObject.FindGameObjectsWithTag("CommandInitiator").Length;
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

    public void RemoveCommand(GameObject command)
    {
        //need more implementation
        command.SetActive(false);
    }
    public void ExecuteCommands()
    {
        //need more implementation
        ResetSteps();
        if (VerifyCommand())
        {
            GameObject startCommand = GameObject.FindGameObjectWithTag("StartCommand");
            startCommand.GetComponent<AbstractCommand>().Execute();
        }
        else
        {
            Debug.Log("Invalid Command, Please Check");
        }

    }

    public void SetSelectedCommand(GameObject commandObject)
    {
        //Debug.Log("Setting" + commandObject.name + "to selected");
        selectedCommand = commandObject;
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
            commandObj.GetComponent<AbstractCommand>().UpdateLink("Disabled");
        }

        GameObject startCommandObj = GameObject.FindGameObjectWithTag("StartCommand");
        AbstractCommand command = startCommandObj.GetComponent<AbstractCommand>();
        HashSet<AbstractCommand> verifiedSet = new HashSet<AbstractCommand>();
        while (command && !verifiedSet.Contains(command))
        {
            verifiedSet.Add(command);
            command.UpdateLink("Default");
            command = command.nextCommand;
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
                command.UpdateLink("Success");
                command.nextCommand.Execute();
            }
            else
            {
                Debug.Log("All Commands Executed");
            }
        }
        else
        {
            Debug.Log("Max Step Exceed");
        }

    }

    public void OnCheckRemainingCommand()
    {
        // StartCommand
        if (GameObject.FindGameObjectsWithTag("StartCommand").Length >= maxCommand.Start)
        {
            GameObject.Find("StartCommandInitiator").GetComponent<CommandInitiator>().setEnabled(false);
        }
        else
        {
            GameObject.Find("StartCommandInitiator").GetComponent<CommandInitiator>().setEnabled(true);
        }
    }
}
