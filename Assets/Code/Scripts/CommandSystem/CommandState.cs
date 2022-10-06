using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Unity.Game.Command
{
    public class CommandState
    {
        #region
        public class CommandSnapshot
        {
            public GameObject commandObject;
            public Vector3 position;
            public bool isActive;
            public AbstractCommand next;
            public List<AbstractCommand> previous;
            public CommandSnapshot(GameObject commandObject = null, Vector3 position = default, bool isActive = false, AbstractCommand next = null, List<AbstractCommand> previous = null)
            {
                this.commandObject = commandObject;
                this.position = position;
                this.isActive = isActive;
                this.next = next;
                this.previous = previous;
            }

        }
        public List<CommandSnapshot> commandSnapshots;

        #endregion

        public static CommandState GetCurrentState()
        {
            CommandState state = new CommandState();
            state.commandSnapshots = new List<CommandSnapshot>();
            SavedElement[] elementsToSave = GameObject.FindObjectsOfType<SavedElement>();


            foreach (SavedElement element in elementsToSave)
            {
                if (element.type == SavedElement.Type.Command)
                {
                    CommandSnapshot snapshot = new CommandSnapshot(
                        element.gameObject,
                        element.gameObject.GetComponentInChildren<Draggable>().dragPosition,
                        element.gameObject.activeSelf,
                        element.gameObject.GetComponent<AbstractCommand>().nextCommand,
                        element.gameObject.GetComponent<AbstractCommand>().previousCommand
                        );
                    state.commandSnapshots.Add(snapshot);
                }
            }
            return state;

        }

        public void LoadCommandState()
        {
            //SavedElement[] elementsToLoad = GameObject.FindObjectsOfType<SavedElement>();

            List<CommandSnapshot> remainingSnapshots = new List<CommandSnapshot>(commandSnapshots);

            // restore all active commands
            foreach (CommandSnapshot snapshot in remainingSnapshots)
            {
                if (snapshot.commandObject)
                {
                    snapshot.commandObject.SetActive(snapshot.isActive);
                    snapshot.commandObject.transform.position = snapshot.position;
                    snapshot.commandObject.GetComponentInChildren<Draggable>().SetisDragging(false);
                }
            }

        }

        public static bool IsSameWithLastState(CommandState a, List<CommandState> list)
        {
            // need further implement
            if (list.Count == 0)
            {
                //Debug.Log("list is empty");
                return false;
            }
            CommandState b = list[list.Count - 1];

            if (a.commandSnapshots.Count != b.commandSnapshots.Count)
            {
                return false;
            }

            //Debug.Log(a.commandSnapshots[0] == b.commandSnapshots[0]);
            return true;
        }
    }
}