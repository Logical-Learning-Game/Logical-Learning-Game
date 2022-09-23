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

    //public List<GameObject> commandObjects;
    public List<GameObject> commands;

    public GameObject selectedCommand;

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

        GetComponent<GridLayoutGroup>().spacing = new Vector2(105f - 15f*GetCommandCount(), 0);
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

        //test method(max command)
        if (GameObject.FindGameObjectsWithTag("StarterCommand").Length > 0)
        {
            GameObject.Find("StartCommandInitiator").GetComponent<CommandInitiator>().setEnabled(false);
        }
        else
        {
            GameObject.Find("StartCommandInitiator").GetComponent<CommandInitiator>().setEnabled(true);
        }

        OnSelectCommand();
        
        
        //
    }
    public void AddCommand(GameObject command)
    { 
        commands.Add(command);
    }

    public void OnSelectCommand()
    {
        foreach(GameObject command in commands)
        {
            if (selectedCommand == command)
            {
                command.GetComponent<CommandSelectable>().SetisSelected(true);
                //command.GetComponent<Draggable>().SetisDraggable(false);
            }
            else
            {
                command.GetComponent<CommandSelectable>().SetisSelected(false);
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
        //Debug.Log(savedCommandStates);
        //Debug.Log(CommandState.GetCurrentState());
        savedCommandStates.Add(CommandState.GetCurrentState());
    }

    public static void UndoCommand()
    {
        if (savedCommandStates.Count > 1)
        {
            savedCommandStates[savedCommandStates.Count - 2].LoadCommandState();
            savedCommandStates.RemoveAt(savedCommandStates.Count - 1);
        }
    }

    //public void RemoveCommand(ICommand command)
    //{
    //    commands.Remove(command);
    //    command.SetActive(false) ;
    //}
    public void RemoveCommand(GameObject command)
    {
        //need more implementation
        command.SetActive(false);
    }
    public void ExecuteCommands()
    {
        //need more implementation
        GameObject startCommand = GameObject.FindGameObjectsWithTag("StarterCommand")[0] ;
        startCommand.GetComponent<AbstractCommand>().Execute();
    }

   

    public void SetSelectedCommand(GameObject commandObject)
    {
        //Debug.Log("Setting" + commandObject.name + "to selected");
        selectedCommand = commandObject;
    }
}
