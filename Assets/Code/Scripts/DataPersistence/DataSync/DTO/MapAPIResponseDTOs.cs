using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using Unity.Game.MapSystem;
using Unity.Game.RuleSystem;

namespace Unity.Game.SaveSystem
{
    public class WorldResponse
    {
        [JsonProperty("world_id")]
        public long WorldId { get; set; }

        [JsonProperty("world_name")]
        public string WorldName { get; set; }

        [JsonProperty("maps")]
        public List<MapResponse> Maps { get; set; }
    }

    public class MapResponse
    {
        [JsonProperty("map_id")]
        public long MapId { get; set; }

        [JsonProperty("map_name")]
        public string MapName { get; set; }

        [JsonProperty("tile")]
        public List<int> Tile { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("map_image_path")]
        public string MapImagePath { get; set; }

        [JsonProperty("difficulty")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Difficulty Difficulty { get; set; }

        [JsonProperty("star_requirement")]
        public int StarRequirement { get; set; }

        [JsonProperty("least_solvable_command_gold")]
        public int LeastSolvableCommandGold { get; set; }

        [JsonProperty("least_solvable_command_silver")]
        public int LeastSolvableCommandSilver { get; set; }

        [JsonProperty("least_solvable_command_bronze")]
        public int LeastSolvableCommandBronze { get; set; }

        [JsonProperty("least_solvable_action_gold")]
        public int LeastSolvableActionGold { get; set; }

        [JsonProperty("least_solvable_action_silver")]
        public int LeastSolvableActionSilver { get; set; }

        [JsonProperty("least_solvable_action_bronze")]
        public int LeastSolvableActionBronze { get; set; }

        [JsonProperty("rules")]
        public List<RuleResponse> Rules { get; set; }

        [JsonProperty("is_pass")]
        public bool IsPass { get; set; }

        [JsonProperty("top_history")]
        public SubmitHistoryDTO TopSubmitHistory { get; set; }
    }

    public class RuleResponse
    {
        [JsonProperty("map_rule_id")]
        public long MapRuleId { get; set; }

        [JsonProperty("rule_name")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RuleName RuleName { get; set; }

        [JsonProperty("rule_theme")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RuleTheme RuleTheme { get; set; }

        [JsonProperty("parameters")]
        public List<int> Parameters { get; set; }
    }
}
