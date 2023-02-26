using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using Unity.Game.Command;
using Unity.Game.Level;
using Unity.Game.MapSystem;
using UnityEngine;

namespace Unity.Game.SaveSystem
{
    public class SessionManager : MonoBehaviour
    {
        [SerializeField]
        private GameDataManager gameDataManager;

        public static GameSession CurrentGameSession { get; set; }

        private void Start()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        }

        private void OnEnable()
        {
            LevelManager.OnMapEnter += StartSession;
            LevelManager.OnMapExit += EndSession;
            CommandManager.OnCommandSubmit += SaveCommand;
        }

        private void OnDisable()
        {
            LevelManager.OnMapEnter -= StartSession;
            LevelManager.OnMapExit -= EndSession;
            CommandManager.OnCommandSubmit -= SaveCommand;
        }

        private void OnApplicationQuit()
        {
            EndSession();
        }

        private void StartSession(Map map)
        {
            CurrentGameSession = new GameSession(map.Id);
            gameDataManager.GameData.SessionHistories.Add(CurrentGameSession, false);
        }

        private async void EndSession()
        {
            CurrentGameSession.EndDatetime = new SerializableDateTime(DateTime.UtcNow);

            // Try send data to backend
            // first check if client can connect to backend service
            var apiClient = new PlayerStatisticAPIClient();

            if (false)
            {
                return;
            }
        
            SerializableDictionary<GameSession, bool> gameSessions = gameDataManager.GameData.SessionHistories;
            foreach (KeyValuePair<GameSession, bool> entry in gameSessions)
            {
                GameSession gameSession = entry.Key;
                bool isAlreadySend = entry.Value;
                if (isAlreadySend)
                {
                    continue;
                }

                GameSessionHistoryRequestDto dto = new StatisticApiDtoMapper().ToDto(gameSession);

                // mock playerId data for testing!!!
                HttpResponseMessage response = await apiClient.SendSessionHistoryData("abcdefg", dto);
                if (response.IsSuccessStatusCode)
                {
                    Debug.Log($"Send success: {response.StatusCode}");
                    gameSessions[entry.Key] = true;
                }
                else
                {
                    Debug.Log($"Send fail: {response.StatusCode}");
                }
            }
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
                SubmitDatetime = new SerializableDateTime(DateTime.UtcNow),
            };

            for (int i = 0; i < context.Rules.Count; i++)
            {
                var ruleHistory = new RuleHistory(context.Rules[i].Id, context.RuleStatus[i]);
                submit.RuleHistories.Add(ruleHistory);
            }

            CurrentGameSession.SubmitHistories.Add(submit);
        }

        private static void GameObjectCommandsToNodeAndEdgeFormat(List<GameObject> commands, out List<CommandNode> commandNodes, out List<CommandEdge> commandEdges)
        {
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
                    destinationNodeIndex = invertedIndexLookup[conditionCommand.linkerCommand.nextCommand];
                    commandEdges.Add(new CommandEdge(sourceNodeIndex, destinationNodeIndex, EdgeType.CONDITIONAL));
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
