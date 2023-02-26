using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Unity.Game.Command;

namespace Unity.Game.SaveSystem
{
    public class GameSessionHistoryRequestDto
    {
        [JsonProperty("map_id")]
        public long MapId { get; set; }

        [JsonProperty("start_datetime")]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime StartDatetime { get; set; }

        [JsonProperty("end_datetime")]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime EndDatetime { get; set; }

        [JsonProperty("submit_histories")]
        public List<SubmitHistoryRequestDto> SubmitHistories { get; set; }
    }

    public class SubmitHistoryRequestDto
    {
        [JsonProperty("is_finited")]
        public bool IsFinited { get; set; }

        [JsonProperty("is_completed")]
        public bool IsCompleted { get; set; }

        [JsonProperty("command_medal")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Medal CommandMedal { get; set; }

        [JsonProperty("action_medal")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Medal ActionMedal { get; set; }

        [JsonProperty("submit_datetime")]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime SubmitDatetime { get; set; }

        [JsonProperty("state_value")]
        public StateValueRequestDto StateValue { get; set; }

        [JsonProperty("command_nodes")]
        public List<CommandNodeRequestDto> CommandNodes { get; set; }

        [JsonProperty("command_edges")]
        public List<CommandEdgeRequestDto> CommandEdges { get; set; }

        [JsonProperty("rules")]
        public List<RuleHistoryRequestDto> RuleHistories { get; set; }
    }

    public class StateValueRequestDto
    {
        [JsonProperty("command_count")]
        public int CommandCount { get; set; }

        [JsonProperty("forward_command_count")]
        public int ForwardCommandCount { get; set; }

        [JsonProperty("left_command_count")]
        public int LeftCommandCount { get; set; }

        [JsonProperty("right_command_count")]
        public int RightCommandCount { get; set; }

        [JsonProperty("back_command_count")]
        public int BackCommandCount { get; set; }

        [JsonProperty("condition_command_count")]
        public int ConditionCommandCount { get; set; }

        [JsonProperty("action_count")]
        public int ActionCount { get; set; }

        [JsonProperty("forward_action_count")]
        public int ForwardActionCount { get; set; }

        [JsonProperty("left_action_count")]
        public int LeftActionCount { get; set; }

        [JsonProperty("right_action_count")]
        public int RightActionCount { get; set; }

        [JsonProperty("back_action_count")]
        public int BackActionCount { get; set; }

        [JsonProperty("condition_action_count")]
        public int ConditionActionCount { get; set; }
    }

    public class RuleHistoryRequestDto
    {
        [JsonProperty("map_rule_id")]
        public long MapRuleId { get; set; }

        [JsonProperty("is_pass")]
        public bool IsPass { get; set; }
    }

    public class CommandNodeRequestDto
    {
        [JsonProperty("node_index")]
        public int NodeIndex { get; set; }

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public CommandType Type { get; set; }

        [JsonProperty("in_game_position")]
        public Vector2FloatDto InGamePosition { get; set; }
    }

    public class CommandEdgeRequestDto
    {
        [JsonProperty("source_node_index")]
        public int SourceNodeIndex { get; set; }

        [JsonProperty("destination_node_index")]
        public int DestinationNodeIndex { get; set; }

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EdgeType Type { get; set; }
    }

    public class Vector2FloatDto
    {
        [JsonProperty("x")]
        public float X { get; set; }

        [JsonProperty("y")]
        public float Y { get; set; }
    }
}
