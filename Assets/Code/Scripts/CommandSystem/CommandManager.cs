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
    
    public List<GameObject> commands;
    // Start is called before the first frame update

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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
    }
    public void InstantiateCommand(GameObject initCommand,PointerEventData eventData)
    {
        GameObject command = Instantiate(initCommand, eventData.position, Quaternion.identity);
        //command.GetComponent<Dragable>().isInstantiator = false;
        command.GetComponent<Dragable>().isDragable = true;
        command.transform.SetParent(gameObject.transform.parent);
        eventData.pointerDrag = command;
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
}
