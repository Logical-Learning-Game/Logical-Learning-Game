using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.Game.Conditions;
using Unity.Game.RuleSystem;
using Newtonsoft.Json;

namespace Unity.Game.MapSystem
{
    public enum Difficulty
    {
        [EnumMember(Value = "easy")]
        EASY,
        [EnumMember(Value = "medium")]
        MEDIUM,
        [EnumMember(Value = "hard")]
        HARD
    }

    public enum TileType { EMPTY, OBSTACLE, GOAL, DOOR, CONDITION_A, CONDITION_B, CONDITION_C, CONDITION_D, CONDITION_E }

    [System.Serializable]
    public class Map
    {
        [JsonProperty("tile")] public uint[] MapData;

        [JsonProperty("map_width")] public int Width;
        [JsonProperty("map_height")] public int Height;

        [JsonProperty("rules")] public List<Rule> MapRules;

        [JsonProperty("star_requirement")] public int StarRequirement;

        [JsonProperty("map_name")] public string MapName;

        [JsonProperty("map_id")] public long Id;

        [JsonProperty("least_solvable_command_gold")] public int LeastSolvableCommandGold;
        [JsonProperty("least_solvable_command_silver")] public int LeastSolvableCommandSilver;
        [JsonProperty("least_solvable_command_bronze")] public int LeastSolvableCommandBronze;
        [JsonProperty("least_solvable_action_gold")] public int LeastSolvableActionGold;
        [JsonProperty("least_solvable_action_silver")] public int LeastSolvableActionSilver;
        [JsonProperty("least_solvable_action_bronze")] public int LeastSolvableActionBronze;

        [JsonProperty("map_image_path")] public string MapImagePath;

        //public Map(uint[] mapData, List<Rule> mapRules, int starRequirement, string mapName, long mapId, int leastSolvableCommand, int leastSolvableAction, int width, int height)
        //{
        //    MapData = mapData;
        //    MapRules = mapRules;
        //    StarRequirement = starRequirement;
        //    MapName = mapName;
        //    Id = mapId;
        //    LeastSolvableCommand = leastSolvableCommand;
        //    LeastSolvableAction = leastSolvableAction;
        //    Width = width;
        //    Height = height;
        //}
        // default constructor for debugging

        //public Map()
        //{
        //    MapData = new uint[]
        //{32,0,48,0,256,16,16,0,0,512,256,0,48,200704,768,7,16,0,0,0 };

        //    MapRules = new List<Rule>() { };
        ////    {
        ////    new LevelClearRule(),
        ////    new ActionLimitRule(value:16,isMore:true),
        ////    new CommandLimitRule(value:7,isMore:false)
        ////};
        //    StarRequirement = 0;
        //    MapName = "0-1";
        //    Id = 1;
        //    //LeastSolvableCommand = 10;
        //    //LeastSolvableAction = 0;
        //    Width = 5;
        //    Height = 4;
        //}

        //public Map(string mapName)
        //{
        //    MapData = new uint[]
        //{32,0,48,0,256,16,16,0,0,512,256,0,48,200704,768,7,16,0,0,0 };

        //    MapRules = new List<Rule>() { };
        ////{
        ////    new LevelClearRule(),
        ////    new ActionLimitRule(value:16,isMore:true),
        ////    new CommandLimitRule(value:7,isMore:false)
        ////};
        //    StarRequirement = 0;
        //    MapName = mapName;
        //    Id = 2;
        //    //LeastSolvableCommand = 10;
        //    //LeastSolvableAction = 0;
        //    Width = 5;
        //    Height = 4;
        //}
    }
}

