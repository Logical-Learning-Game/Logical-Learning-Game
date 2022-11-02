using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Game.Map
{
    public class GoalTile : Tile
    {

        public override bool IsEnterable()
        {
            return true;
        }

        public override void OnTileEntered()
        {
            base.OnTileEntered();
            Debug.Log("Enter goal, this level should be pass");
        }
    }
}