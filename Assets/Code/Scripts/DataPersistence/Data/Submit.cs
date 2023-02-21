using System;
using System.Collections.Generic;
using Unity.Game.Command;

namespace Unity.Game.SaveSystem
{
    [Serializable]
    public enum Medal 
    { 
        NONE, 
        BRONZE, 
        SILVER, 
        GOLD 
    }

    [Serializable]
    public class GameSession
    {
        public long MapId { get; set; }
        public SerializableDateTime StartDatetime { get; set; }
        public SerializableDateTime? EndDatetime { get; set;}
        public List<Submit> SubmitHistories { get; }

        public GameSession(long mapId)
        {
            MapId = mapId;
            StartDatetime = new SerializableDateTime(DateTime.Now);
            SubmitHistories = new List<Submit>();
        }
    }

    [Serializable]
    public class Submit
    {
        public bool IsFinited { get; set; }
        public bool IsCompleted { get; set; }
        public Medal CommandMedal { get; set; }
        public Medal ActionMedal { get; set; }
        public SerializableDateTime SubmitDatetime { get; set; }
        public StateValue StateValue { get; set; }
        public List<CommandNode> CommandNodes { get; set; }
        public List<CommandEdge> CommandEdges { get; set; }
        public List<RuleHistory> RuleHistories { get; set; }
        
        public Submit() { }

        public Submit(List<CommandNode> commandPatterns, List<CommandEdge> commandEdge, bool isFinited, bool isCompleted, List<RuleHistory> ruleHistories, Medal commandMedal, Medal actionMedal, StateValue stateValue)
        {
            CommandNodes = commandPatterns;
            CommandEdges = commandEdge;
            IsFinited = isFinited;
            IsCompleted = isCompleted;
            RuleHistories = ruleHistories;
            CommandMedal = commandMedal;
            ActionMedal = actionMedal;
            StateValue = stateValue;
            SubmitDatetime = new SerializableDateTime(DateTime.Now);
        }
    }

    [Serializable]
    public class RuleHistory
    {
        public long MapRuleId { get; set; }
        public bool IsPass { get; set; }

        public RuleHistory(long mapRuleId, bool isPass)
        {
            MapRuleId = mapRuleId;
            IsPass = isPass;
        }
    }


    [Serializable]
    public class CommandNode
    {
        public int Index { get; set; }
        public CommandType Type { get; set; }
        public float X { get; set; }
        public float Y { get; set; }

        public CommandNode(CommandType type, float x, float y)
        {
            Type = type;
            X = x;
            Y = y;
        }
    }


    [Serializable]
    public class CommandEdge
    {
        public int SourceCommandIndex { get; set; }
        public int DestinationCommandIndex { get; set; }
        EdgeType Type { get; set; }
        public CommandEdge(int sourceCommandIndex, int destinationCommandIndex, EdgeType type)
        {
            SourceCommandIndex = sourceCommandIndex;
            DestinationCommandIndex = destinationCommandIndex;
            Type = type;
        }
    }
}
