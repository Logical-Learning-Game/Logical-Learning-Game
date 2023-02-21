using System;
using System.Collections.Generic;
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

        private void Awake()
        {
            
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
            
        }

        private void StartSession(Map map)
        {
            CurrentGameSession = new GameSession(map.Id);
            gameDataManager.GameData.SessionHistories.Add(CurrentGameSession, false);
        }

        private void EndSession()
        {
            // Try send data to backend
        }

        private void SaveCommand(SubmitContext context)
        {
            // Change GameObject command to new format
            List<CommandNode> commandNodes;
            List<CommandEdge> commandEdges;
            GameObjectCommandsToNodeAndEdgeFormat(context.Commands, out commandNodes, out commandEdges);

            var submit = new Submit
            {
                CommandNodes = commandNodes,
                CommandEdges = commandEdges,
                IsFinited = context.IsFinited,
                IsCompleted = context.IsCompleted,
                CommandMedal = context.CommandMedal,
                ActionMedal = context.ActionMedal,
                StateValue = context.StateValue,
                RuleHistories = new List<RuleHistory>(),
                SubmitDatetime = new SerializableDateTime(DateTime.Now),
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

            //TODO
        }
    }
}
