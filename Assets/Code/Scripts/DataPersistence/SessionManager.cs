using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using Unity.Game.Command;
using Unity.Game.Level;
using Unity.Game.MapSystem;
using UnityEngine;
using Unity.Game.UI;

namespace Unity.Game.SaveSystem
{
    public class SessionManager : MonoBehaviour
    {
        [SerializeField]
        private GameDataManager GameDataManager;

        [SerializeField]
        private SaveManager SaveManager;

        public static GameSession CurrentGameSession;

        public static event Action<SubmitHistory> OnCommandSubmit;

        private void OnEnable()
        {
            LevelManager.OnMapEnter += StartSession;
            LevelManager.OnMapExit += EndSession;
            GameScreen.GameRestarted += EndSession;
            CommandManager.OnCommandSubmit += SaveCommand;
        }

        private void OnDisable()
        {
            LevelManager.OnMapEnter -= StartSession;
            LevelManager.OnMapExit -= EndSession;
            GameScreen.GameRestarted -= EndSession;
            CommandManager.OnCommandSubmit -= SaveCommand;
        }

        private void Awake()
        {
            if (GameDataManager == null || SaveManager == null)
            {
                GameObject gameDataManagerGameObject = GameObject.Find("GameDataManager");
                GameDataManager = gameDataManagerGameObject.GetComponent<GameDataManager>();
                SaveManager = gameDataManagerGameObject.GetComponentInParent<SaveManager>();
            }
        }
        private void OnApplicationQuit()
        {
            EndSession();
        }

        private void StartSession(Map map)
        {
            Debug.Log("Session is created");
            CurrentGameSession = new GameSession(map.Id);
            
        }

        private void EndSession(bool isSameMap)
        {
            if (isSameMap) return;

            EndSession();
        }

        private async void EndSession()
        {
            Debug.Log("Session is ending");
            if (CurrentGameSession == null)
            {
                SaveManager.SaveGame();
                return;
            }
            if(CurrentGameSession.SubmitHistories.Count == 0)
            {
                SaveManager.SaveGame();
                CurrentGameSession = null;
                return;
            }
            GameDataManager.GameData.SessionHistories.Add(new SessionStatus(CurrentGameSession, false));
            CurrentGameSession.EndDatetime = DateTime.Now;
            CurrentGameSession = null;

            await GameDataManager.SendGameData();

            SaveManager.SaveGame();
        }

        private void SaveCommand(SubmitContext context)
        {
            List<CommandNode> commandNodes;
            List<CommandEdge> commandEdges;
            GameObjectCommandsToNodeAndEdgeFormat(context.Commands, out commandNodes, out commandEdges);

            var submit = new SubmitHistory
            {
                CommandNodes = commandNodes,
                CommandEdges = commandEdges,
                IsFinited = context.IsFinited,
                IsCompleted = context.IsCompleted,
                CommandMedal = context.CommandMedal,
                ActionMedal = context.ActionMedal,
                StateValue = context.StateValue,
                RuleHistories = new List<RuleHistory>(),
                SubmitDatetime = DateTime.Now,
            };

            for (int i = 0; i < context.Rules.Count; i++)
            {
                var ruleHistory = new RuleHistory(context.Rules[i].Id, context.RuleStatus[i], context.Rules[i].Theme);
                submit.RuleHistories.Add(ruleHistory);
            }

            CurrentGameSession.SubmitHistories.Add(submit);
            // compare and add submit to topsubmits
            if(GameDataManager.GameData.SubmitBest.TryGetValue(CurrentGameSession.MapId, out SubmitHistory oldSubmit))
            {
                GameDataManager.GameData.SubmitBest[CurrentGameSession.MapId] = SubmitHistory.GetBestSubmit(new List<SubmitHistory>() { submit,oldSubmit });
            }
            else
            {
                GameDataManager.GameData.SubmitBest.Add(CurrentGameSession.MapId, submit);
            }
            OnCommandSubmit?.Invoke(submit);
        }

        private static void GameObjectCommandsToNodeAndEdgeFormat(List<GameObject> commands, out List<CommandNode> commandNodes, out List<CommandEdge> commandEdges)
        {
            Debug.Log(commands.Count);

            commandNodes = new List<CommandNode>();
            commandEdges = new List<CommandEdge>();

            var abstractCommands = new List<AbstractCommand>();
            var invertedIndexLookup = new Dictionary<AbstractCommand, int>();

            for (int i = 0; i < commands.Count; i++)
            {
                AbstractCommand abstractCommand = commands[i].GetComponent<AbstractCommand>();
                Vector2 position = commands[i].transform.localPosition;

                commandNodes.Add(new CommandNode(i, abstractCommand.type, position.x, position.y));

                abstractCommands.Add(abstractCommand);
                invertedIndexLookup.Add(abstractCommand, i);
            }

            for (int i = 0; i < commandNodes.Count; i++)
            {
                int sourceNodeIndex = i;
                int destinationNodeIndex;
                CommandNode node = commandNodes[i];
                AbstractCommand abstractCommand = abstractCommands[i];

                if (node.Type is CommandType.CONDITIONAL_A or
                    CommandType.CONDITIONAL_B or
                    CommandType.CONDITIONAL_C or
                    CommandType.CONDITIONAL_D or
                    CommandType.CONDITIONAL_E)
                {
                    var conditionCommand = abstractCommand as ConditionCommand;

                    // conditional edge
                    if(conditionCommand.linkerCommand.nextCommand != null)
                    {
                        destinationNodeIndex = invertedIndexLookup[conditionCommand.linkerCommand.nextCommand];
                        commandEdges.Add(new CommandEdge(sourceNodeIndex, destinationNodeIndex, EdgeType.CONDITIONAL));
                    }
                }
                    
                   

                if (abstractCommand.nextCommand != null)
                {
                    destinationNodeIndex = invertedIndexLookup[abstractCommand.nextCommand];
                    commandEdges.Add(new CommandEdge(sourceNodeIndex, destinationNodeIndex, EdgeType.MAIN));
                }
            }
        }
    }
}
