using System.Collections;
using System.Collections.Generic;
using Unity.Game.Conditions;
using Unity.Game.RuleSystem;

namespace Unity.Game.MapSystem
{
    public enum TileType { EMPTY, OBSTACLE, GOAL, DOOR, CONDITION_A, CONDITION_B, CONDITION_C, CONDITION_D, CONDITION_E }
    [System.Serializable]
    public class Map
    {

        public uint[,] MapData = new uint[,]
        {
            { 32,0,48,0,256 },
            { 16,16,0,0,512 },
            { 256,0,48,200704,768 },
            { 7,16,0,0,0 }
        };

        public List<Rule> MapRules = new List<Rule>()
        {
            new LevelClearRule(),
            new ActionLimitRule(value:16,isMore:true),
            new CommandLimitRule(value:7,isMore:false)
        };

        public int StarRequirement = 0;

        public string MapName = "0-1";

        public string MapId;

        public int LeastSolvableCommand;

        public int LeastSolvableAction;

        public Map(uint[,] mapData, List<Rule> mapRules, int starRequirement, string mapName, string mapId, int leastSolvableCommand, int leastSolvableAction)
        {
            MapData = mapData;
            MapRules = mapRules;
            StarRequirement = starRequirement;
            MapName = mapName;
            MapId = mapId;
            LeastSolvableCommand = leastSolvableCommand;
            LeastSolvableAction = leastSolvableAction;
        }
        // default constructor for debugging
        public Map()
        {
            MapData = new uint[,]
        {
            { 32,0,48,0,256 },
            { 16,16,0,0,512 },
            { 256,0,48,200704,768 },
            { 7,16,0,0,0 }
        };
            MapRules = new List<Rule>()
        {
            new LevelClearRule(),
            new ActionLimitRule(value:16,isMore:true),
            new CommandLimitRule(value:7,isMore:false)
        };
            StarRequirement = 0;
            MapName = "0-1";
            MapId = "TestMap-01";
            LeastSolvableCommand = 10;
            LeastSolvableAction = 0;
        }

        public int Width { get { return MapData.GetLength(0); } }
        public int Height { get { return MapData.GetLength(1); } }

    }
}

