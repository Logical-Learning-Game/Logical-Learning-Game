using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Game.Conditions;

namespace Unity.Game.Map
{
    public class ConditionTile : Tile
    {
        // Condition, will be implement later
        Condition tileCondition = new Condition();
        public override bool IsEnterable()
        {
            return true;
        }

        public override void OnTileEntered()
        {
            base.OnTileEntered();
            Debug.Log("Entered Condition Tile, Character should memorize this pattern");
        }
    }
}