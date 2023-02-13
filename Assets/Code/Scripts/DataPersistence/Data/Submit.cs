using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Game.Command;
using Unity.Game.RuleSystem;

namespace Unity.Game.SaveSystem
{
    [System.Serializable]
    public enum Medal { NONE, BRONZE, SILVER, GOLD }
    [System.Serializable]
    public class CommandPattern
    {
        CommandType type;
        float[] position;

        public CommandPattern(CommandType type, float[] position)
        {
            this.type = type;
            this.position = position;
        }
    }
    [System.Serializable]
    public class CommandEdge
    {
        
        [SerializeField] CommandPattern commandStart;
        [SerializeField] CommandPattern commandEnd;
        EdgeType edgeType;
        public CommandEdge(CommandPattern start, CommandPattern end, EdgeType type)
        {
            commandStart = start;
            commandEnd = end;
            edgeType = type;
        }
    }
    [System.Serializable]
    public class Submit
    {
        public string userId;
        public string mapId;
        public string sessionId;
        public SerializableDateTime submitDate;
        public List<CommandPattern> commandPatterns;
        public List<CommandEdge> commandEdge;
        public bool isFinited;
        public bool isCompleted;
        public List<Rule> Rules;
        public bool[] ruleStatus;
        public Medal commandMedal;
        public Medal actionMedal;
        public StateValue stateValue;

        public Submit(string userId, string mapId, string sessionId,  List<CommandPattern> commandPatterns, List<CommandEdge> commandEdge, bool isFinited, bool isCompleted, List<Rule> rules, bool[] ruleStatus, Medal commandMedal, Medal actionMedal, StateValue stateValue)
        {
            this.userId = userId;
            this.mapId = mapId;
            this.sessionId = sessionId;
            this.commandPatterns = commandPatterns;
            this.commandEdge = commandEdge;
            this.isFinited = isFinited;
            this.isCompleted = isCompleted;
            Rules = rules;
            this.ruleStatus = ruleStatus;
            this.commandMedal = commandMedal;
            this.actionMedal = actionMedal;
            this.stateValue = stateValue;
            this.submitDate = new SerializableDateTime(DateTime.Now);
        }
    }

}
