using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Game.Command;
using Unity.Game.RuleSystem;

namespace Unity.Game.SaveSystem
{
    public enum Medal { NONE, BRONZE, SILVER, GOLD }
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
    public class CommandEdge
    {

        CommandPattern commandStart;
        CommandPattern commandEnd;
        EdgeType edgeType;
        public CommandEdge(CommandPattern start, CommandPattern end, EdgeType type)
        {
            commandStart = start;
            commandEnd = end;
            edgeType = type;
        }
    }
    public class Submit
    {
        string userId;
        string mapId;
        string sessionId;
        DateTime submitDate;
        List<CommandPattern> commandPatterns;
        List<CommandEdge> commandEdge;
        bool isFinited;
        bool isCompleted;
        List<Rule> Rules;
        bool[] ruleStatus;
        Medal commandMedal;
        Medal actionMedal;
        StateValue stateValue;

    }

}
