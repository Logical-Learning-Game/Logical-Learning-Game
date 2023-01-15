using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Game.MapSystem
{
    public class EmptyTile : Tile
    {

        public override bool IsEnterable()
        {
            return true;
        }

        public override void OnTileEntered()
        {
            base.OnTileEntered();
        }
    }
}