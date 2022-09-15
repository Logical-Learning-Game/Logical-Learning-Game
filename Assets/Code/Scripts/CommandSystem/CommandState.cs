using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandState
{
    #region
    public List<Vector3> commandPositions;
    //public List<bool> commandIsActives;
    #endregion

    public static CommandState GetCurrentState()
    {
        CommandState state = new CommandState();
        SavedElement[] elementsToSave = GameObject.FindObjectsOfType<SavedElement>();
        state.commandPositions = new List<Vector3>();
        //state.commandIsActives = new List<bool>();

        foreach (SavedElement element in elementsToSave)
        {
            if (element.type == SavedElement.Type.Command)
            {
                state.commandPositions.Add(element.GetComponent<Dragable>().dragPosition);
                //state.commandIsActives.Add(element.gameObject.activeSelf);
            }
        }
        return state;
    }

    public void LoadCommandState()
    {
        SavedElement[] elementsToLoad = GameObject.FindObjectsOfType<SavedElement>();
        List<Vector3> remainingCommandPositions = new List<Vector3>(commandPositions);
        //List<bool> remainingCommandIsActives = new List<bool>(commandIsActives);
        
        foreach (SavedElement elementToLoad in elementsToLoad)
        {
            if (elementToLoad.type == SavedElement.Type.Command)
            {
                elementToLoad.transform.position = remainingCommandPositions[0];
                elementToLoad.GetComponent<Dragable>().isDragging = false;
                remainingCommandPositions.RemoveAt(0);

                //elementToLoad.gameObject.SetActive(remainingCommandIsActives[0]);
                //remainingCommandIsActives.RemoveAt(0);
            }
        }
    }
}
