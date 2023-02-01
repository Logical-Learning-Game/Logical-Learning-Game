using System.Collections;
using System.Collections.Generic;
using Unity.Game.Conditions;
using Unity.Game.RuleSystem;

namespace Unity.Game.MapSystem
{
    public enum TileType { EMPTY, OBSTACLE, GOAL, DOOR, CONDITION_A, CONDITION_B, CONDITION_C, CONDITION_D, CONDITION_E }
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
            //new NormalClearRule(),
            //new CommandLimitRule(),
            //new ActionLimitRule()
        };

        public int StarRequirement = 0;

        public string MapName = "0-1";

        public List<int> LeastSolvableCommand = new List<int>()
        {
            1,2,3
        };

        public List<int> LeastSolvableAction = new List<int>()
        {
            6,9,12
        };
        public int Width { get { return MapData.GetLength(0); } }
        public int Height { get { return MapData.GetLength(1); } }

        public (int[], int[]) GetPlayerInit()
        {
            int[] playerPosition = new int[2] { 0, 0 };
            int[] playerRotation = new int[2] { 0, 0 };
            for (int i = 0; i < MapData.GetLength(0); i++)
            {
                for (int j = 0; j < MapData.GetLength(1); j++)
                {
                    if ((MapData[i, j] & 0b1) == 1)
                    {
                        playerPosition = new int[2] { i, j };
                        playerRotation = new int[2] { (int)(MapData[i, j] >> 2) & 0b1, (int)(MapData[i, j] >> 1) & 0b1 };

                        return (playerPosition, playerRotation);
                    }
                }
            }
            return (playerPosition, playerRotation);
        }

        public HashSet<ConditionSign> GetUniqueConditions()
        {
            HashSet<ConditionSign> uniqueConditions = new HashSet<ConditionSign>();
            foreach (uint data in MapData)
            {
                uint tile = data >> 4 & 0b1111;
                if (tile == 0b0011)
                {
                    uniqueConditions.Add(ConditionSign.A);
                }
                else if (tile == 0b0100)
                {
                    uniqueConditions.Add(ConditionSign.B);
                }
                else if (tile == 0b0101)
                {
                    uniqueConditions.Add(ConditionSign.C);
                }
                else if (tile == 0b0110)
                {
                    uniqueConditions.Add(ConditionSign.D);
                }
                else if (tile == 0b0111)
                {
                    uniqueConditions.Add(ConditionSign.E);
                }
            }
            return uniqueConditions;
        }
    }
}

