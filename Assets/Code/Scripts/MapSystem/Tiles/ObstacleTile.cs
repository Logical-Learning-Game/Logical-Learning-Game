using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Game.MapSystem
{
    public class ObstacleTile : Tile
    {

        public override bool IsEnterable(Tuple<int, int> comingDirection)
        {
            CreateTileAura();
            return false;
        }

        public override void OnTileEntered()
        {
            base.OnTileEntered();
            //Debug.Log("Character Should not enter ObstacleTile");
        }
    }
}