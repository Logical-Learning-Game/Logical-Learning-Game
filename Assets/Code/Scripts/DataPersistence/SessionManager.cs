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

        [SerializeField]
        private SaveManager SaveManager;

        public static GameSession CurrentGameSession;

        public static event Action<SubmitHistory> OnCommandSubmit;

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
            Debug.Log("Session is created");
            CurrentGameSession = new GameSession(map.Id);
            gameDataManager.GameData.SessionHistories.Add(new SessionStatus(CurrentGameSession, false));
        }

        private async void EndSession()
        {
            if (CurrentGameSession == null)
            {
                SaveManager.SaveGame();
                return;
            }

            CurrentGameSession.EndDatetime = DateTime.Now;
            CurrentGameSession = null;

            // Try send data to backend
            var apiClient = new APIClient();

            // first check if client can connect to backend service
            bool haveConnectionToServer = await apiClient.ConnectionCheck();
            if (!haveConnectionToServer)
            {
                return;
            }

            // send session history to server
            List<SessionStatus> gameSessionWithSendStatus = gameDataManager.GameData.SessionHistories;

            foreach (SessionStatus entry in gameSessionWithSendStatus)
            {
                GameSession gameSession = entry.Session;
                bool isAlreadySend = entry.Status;
                if (isAlreadySend)
                {
                    continue;
                }

                GameSessionHistoryRequest dto = new GameSessionDTOMapper().ToDTO(gameSession);

                // mock playerId data for testing!!!
                try
                {
                    await apiClient.SendSessionHistoryData("abcdefg", dto);
                    entry.Status = true;
                }
                catch (APIException ex)
                {
                    Debug.LogErrorFormat("Receive a non successful status code from server while sending session history: {0}", ex.Content);
                    break;
                }
                catch (HttpRequestException ex)
                {
                    Debug.LogErrorFormat("An error occurred while making http request to create session history endpoint: {0}", ex);
                    break;
                }
            }

            // send submit best to server
            Dictionary<long, SubmitHistory> submitBest = gameDataManager.GameData.SubmitBest;
            var topSubmitHistoryRequests = new List<TopSubmitHistoryRequest>();
            var submitHistoryDTOMapper = new SubmitHistoryDTOMapper();

            foreach (KeyValuePair<long, SubmitHistory> entry in submitBest)
            {
                long mapId = entry.Key;
                SubmitHistory submitHistory = entry.Value;

                var topSubmitHistoryRequest = new TopSubmitHistoryRequest
                {
                    MapId = mapId,
                    TopSubmitHistory = submitHistoryDTOMapper.ToDTO(submitHistory)
                };

                topSubmitHistoryRequests.Add(topSubmitHistoryRequest);
            }

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
            if(gameDataManager.GameData.SubmitBest.TryGetValue(CurrentGameSession.MapId, out SubmitHistory oldSubmit))
            {
                gameDataManager.GameData.SubmitBest[CurrentGameSession.MapId] = SubmitHistory.GetBestSubmit(new List<SubmitHistory>() { submit,oldSubmit });
            }
            else
            {
                gameDataManager.GameData.SubmitBest.Add(CurrentGameSession.MapId, submit);
            }
            OnCommandSubmit?.Invoke(submit);
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
