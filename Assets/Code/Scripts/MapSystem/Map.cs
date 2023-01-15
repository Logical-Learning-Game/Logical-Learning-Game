using System.Collections;
using System.Collections.Generic;
using GlobalConfig;

namespace Unity.Game.Map
{
    public enum TileType { EMPTY, OBSTACLE, GOAL, DOOR, CONDITION_A, CONDITION_B, CONDITION_C, CONDITION_D, CONDITION_E }
    public class Map
    {
        public TileType[,] TileArray = new TileType[,]
       {
           { TileType.EMPTY,TileType.OBSTACLE,TileType.OBSTACLE,TileType.GOAL },
           { TileType.EMPTY,TileType.OBSTACLE,TileType.OBSTACLE,TileType.EMPTY },
           { TileType.EMPTY,TileType.OBSTACLE,TileType.OBSTACLE,TileType.EMPTY },
           { TileType.CONDITION_A,TileType.CONDITION_C,TileType.CONDITION_D,TileType.CONDITION_B },
       };

        public int Width { get { return TileArray.GetLength(0); } }
        public int Height { get { return TileArray.GetLength(1); } }
        //Item Implementation
        //public Item items[] = List<Item>();

    }

}