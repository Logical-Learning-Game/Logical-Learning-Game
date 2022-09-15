using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CommandManager : MonoBehaviour
{
    public static CommandManager Instance { get; private set; }


    public static List<CommandState> savedCommandStates = new List<CommandState>();

    public List<GameObject> commandObjects;
    public List<AbstractCommand> commands;
    
    // Start is called before the first frame update

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SaveCommandState();
            commands = new List<AbstractCommand>();
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

        if (GameObject.FindGameObjectsWithTag("StarterCommand").Length > 0)
        {
            GameObject.Find("StarterCommandInitiator").GetComponent<StartCommandInitiator>().setEnabled(false);
        }
        else
        {
            GameObject.Find("StarterCommandInitiator").GetComponent<StartCommandInitiator>().setEnabled(true);

        }
    }
    public void InstantiateCommand(AbstractCommand command)
    { 
        commands.Add(command);
    }

    private int GetCommandCount()
    {
        return GameObject.FindGameObjectsWithTag("CommandInitiator").Length;
    }

    public static void SaveCommandState()
    {
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
        GameObject startCommandObject = GameObject.FindGameObjectsWithTag("StarterCommand")[0];
        AbstractCommand startCommand = GetCommandFromGameObject(startCommandObject);
        startCommand.Execute();
    }

    public AbstractCommand GetCommandFromGameObject(GameObject commandObject)
    {
        //need more implementation
        foreach (AbstractCommand command in commands)
        {
            if (command.GetCommandObject().Equals(commandObject))
            {
                return command;
            }
        }
        return null;
    }
}
