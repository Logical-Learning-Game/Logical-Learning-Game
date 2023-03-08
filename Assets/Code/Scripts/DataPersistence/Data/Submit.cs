using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using Unity.Game.Command;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Unity.Game.RuleSystem;
using Unity.Game.MapSystem;

namespace Unity.Game.SaveSystem
{
    [Serializable]
    public enum Medal
    {
        [EnumMember(Value = "none")]
        NONE,
        [EnumMember(Value = "bronze")]
        BRONZE,
        [EnumMember(Value = "silver")]
        SILVER,
        [EnumMember(Value = "gold")]
        GOLD
    }


    [Serializable]
    public class GameSession : IEquatable<GameSession>
    {

        [JsonProperty("map_id")] public long MapId;
        [JsonProperty("start_datetime")][JsonConverter(typeof(IsoDateTimeConverter))] public DateTime StartDatetime;
        [JsonProperty("end_datetime")][JsonConverter(typeof(IsoDateTimeConverter))] public DateTime EndDatetime;
        [JsonProperty("submit_histories")] public List<SubmitHistory> SubmitHistories;

        public GameSession(long mapId)
        {
            MapId = mapId;
            StartDatetime = DateTime.Now;
            SubmitHistories = new List<SubmitHistory>();
        }

        public bool Equals(GameSession other)
        {
            if (other == null)
            {
                return false;
            }

            if (this.MapId != other.MapId)
            {
                return false;
            }

            if (!this.StartDatetime.Equals(other.StartDatetime))
            {
                return false;
            }

            if (!this.EndDatetime.Equals(other.EndDatetime))
            {
                return false;
            }

            if (this.SubmitHistories.Count != other.SubmitHistories.Count)
            {
                return false;
            }

            for (int i = 0; i < this.SubmitHistories.Count; i++)
            {
                if (!this.SubmitHistories[i].Equals(other.SubmitHistories[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }

    [Serializable]
    public class SubmitHistory
    {
        [JsonProperty("is_finited")] public bool IsFinited;
        [JsonProperty("is_completed")] public bool IsCompleted;
        [JsonProperty("command_medal")][JsonConverter(typeof(StringEnumConverter))] public Medal CommandMedal;
        [JsonProperty("action_medal")][JsonConverter(typeof(StringEnumConverter))] public Medal ActionMedal;
        [JsonProperty("submit_datetime")][JsonConverter(typeof(IsoDateTimeConverter))] public DateTime SubmitDatetime;

        [JsonProperty("state_value")] public StateValue StateValue;
        [JsonProperty("command_nodes")] public List<CommandNode> CommandNodes;
        [JsonProperty("command_edges")] public List<CommandEdge> CommandEdges;
        [JsonProperty("rules")] public List<RuleHistory> RuleHistories;

        public SubmitHistory() { }

        public SubmitHistory(List<CommandNode> commandPatterns, List<CommandEdge> commandEdge, bool isFinited, bool isCompleted, List<RuleHistory> ruleHistories, Medal commandMedal, Medal actionMedal, StateValue stateValue)
        {
            CommandNodes = commandPatterns;
            CommandEdges = commandEdge;
            IsFinited = isFinited;
            IsCompleted = isCompleted;
            RuleHistories = ruleHistories;
            CommandMedal = commandMedal;
            ActionMedal = actionMedal;
            StateValue = stateValue;
            SubmitDatetime = DateTime.Now;
        }

        public static (Medal,Medal) GetMedal(StateValue currentValue,Map map)
        {
            Medal commandMedal = Medal.NONE;
            Medal actionMedal = Medal.NONE;

            if (currentValue.CommandCount <= map.LeastSolvableCommandGold)
            {
                commandMedal = Medal.GOLD;
            }else if (currentValue.CommandCount <= map.LeastSolvableCommandSilver)
            {
                commandMedal = Medal.SILVER;
            }
            else if (currentValue.CommandCount <= map.LeastSolvableCommandBronze)
            {
                commandMedal = Medal.BRONZE;
            }

            if (currentValue.ActionCount <= map.LeastSolvableActionGold)
            {
                actionMedal = Medal.GOLD;
            }
            else if (currentValue.CommandCount <= map.LeastSolvableActionSilver)
            {
                actionMedal = Medal.SILVER;
            }
            else if (currentValue.CommandCount <= map.LeastSolvableActionBronze)
            {
                actionMedal = Medal.BRONZE;
            }

            return (commandMedal, actionMedal);
        }

        public static SubmitHistory GetBestSubmit(List<SubmitHistory> AllSubmits)
        {
            if (AllSubmits == null || AllSubmits?.Count == 0) return null;

            SubmitHistory bestSubmit = AllSubmits[0];

            foreach (SubmitHistory submit in AllSubmits)
            {
                // Check if the submit is completed
                if (submit.IsCompleted && !bestSubmit.IsCompleted)
                {
                    bestSubmit = submit;
                }
                else if (submit.IsCompleted && bestSubmit.IsCompleted)
                {
                    // Compare RuleHistories
                    bool allPass = true;
                    for (int i = 0; i < submit.RuleHistories.Count; i++)
                    {
                        if (!submit.RuleHistories[i].IsPass && bestSubmit.RuleHistories[i].IsPass)
                        {
                            allPass = false;
                            break;
                        }
                        else if (!submit.RuleHistories[i].IsPass && !bestSubmit.RuleHistories[i].IsPass)
                        {
                            // First child is more important
                            if (i == 0)
                            {
                                bestSubmit = submit;
                                break;
                            }
                        }
                    }

                    if (allPass)
                    {
                        // Compare CommandMedal and ActionMedal
                        if (submit.CommandMedal > bestSubmit.CommandMedal)
                        {
                            bestSubmit = submit;
                        }
                        else if (submit.CommandMedal == bestSubmit.CommandMedal && submit.ActionMedal > bestSubmit.ActionMedal)
                        {
                            bestSubmit = submit;
                        }
                        else if (submit.CommandMedal == bestSubmit.CommandMedal && submit.ActionMedal == bestSubmit.ActionMedal)
                        {
                            // Compare StateValue
                            if (submit.StateValue.CommandCount < bestSubmit.StateValue.CommandCount)
                            {
                                bestSubmit = submit;
                            }
                            else if (submit.StateValue.CommandCount == bestSubmit.StateValue.CommandCount && submit.StateValue.ActionCount < bestSubmit.StateValue.ActionCount)
                            {
                                bestSubmit = submit;
                            }
                        }
                    }
                }
            }

            return bestSubmit;
        }
    }

    [Serializable]
    public class RuleHistory
    {
        [JsonProperty("map_rule_id")] public long MapRuleId;
        [JsonProperty("theme")][JsonConverter(typeof(StringEnumConverter))] public RuleTheme Theme;
        [JsonProperty("is_pass")] public bool IsPass;

        public RuleHistory(long mapRuleId, bool isPass, RuleTheme theme)

        {
            MapRuleId = mapRuleId;
            Theme = theme;
            IsPass = isPass;
        }
    }


    [Serializable]
    public class CommandNode
    {
        [JsonProperty("index")] public int Index;
        [JsonProperty("type")][JsonConverter(typeof(StringEnumConverter))] public CommandType Type;
        [JsonProperty("x")] public float X;
        [JsonProperty("y")] public float Y;

        public CommandNode(int index, CommandType type, float x, float y)
        {
            Index = index;
            Type = type;
            X = x;
            Y = y;
        }
    }


    [Serializable]
    public class CommandEdge
    {
        [JsonProperty("source_node_index")] public int SourceCommandIndex;
        [JsonProperty("destination_node_index")] public int DestinationCommandIndex;
        [JsonProperty("type")][JsonConverter(typeof(StringEnumConverter))] public EdgeType Type;
        public CommandEdge(int sourceCommandIndex, int destinationCommandIndex, EdgeType type)
        {
            SourceCommandIndex = sourceCommandIndex;
            DestinationCommandIndex = destinationCommandIndex;
            Type = type;
        }
    }
}
