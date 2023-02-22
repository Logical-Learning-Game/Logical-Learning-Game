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

        public uint[] MapData = new uint[]
        {32,0,48,0,256,16,16,0,0,512,256,0,48,200704,768,7,16,0,0,0 };

        public int Width;
        public int Height;

        public List<Rule> MapRules = new List<Rule>()
        {
            new LevelClearRule(),
            new ActionLimitRule(value:16,isMore:true),
            new CommandLimitRule(value:7,isMore:false)
        };

        public int StarRequirement = 0;

        public string MapName = "0-1";

        public long Id;

        public int LeastSolvableCommand;

        public int LeastSolvableAction;

        public Map(uint[] mapData, List<Rule> mapRules, int starRequirement, string mapName, long mapId, int leastSolvableCommand, int leastSolvableAction, int width, int height)
        {
            MapData = mapData;
            MapRules = mapRules;
            StarRequirement = starRequirement;
            MapName = mapName;
            Id = mapId;
            LeastSolvableCommand = leastSolvableCommand;
            LeastSolvableAction = leastSolvableAction;
            Width = width;
            Height = height;
        }
        // default constructor for debugging
        public Map()
        {
            MapData = new uint[]
        {32,0,48,0,256,16,16,0,0,512,256,0,48,200704,768,7,16,0,0,0 };

            MapRules = new List<Rule>()
        {
            new LevelClearRule(),
            new ActionLimitRule(value:16,isMore:true),
            new CommandLimitRule(value:7,isMore:false)
        };
            StarRequirement = 0;
            MapName = "0-1";
            Id = 1;
            LeastSolvableCommand = 10;
            LeastSolvableAction = 0;
            Width = 5;
            Height = 4;
        }

        public Map(string mapName)
        {
            MapData = new uint[]
        {32,0,48,0,256,16,16,0,0,512,256,0,48,200704,768,7,16,0,0,0 };

            MapRules = new List<Rule>()
        {
            new LevelClearRule(),
            new ActionLimitRule(value:16,isMore:true),
            new CommandLimitRule(value:7,isMore:false)
        };
            StarRequirement = 0;
            MapName = mapName;
            Id = 2;
            LeastSolvableCommand = 10;
            LeastSolvableAction = 0;
            Width = 5;
            Height = 4;
        }
    }
}

