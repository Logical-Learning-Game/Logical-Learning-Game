using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Game.Map
{
    public class ObstracleTile : Tile
    {

        public override bool IsEnterable()
        {
            return false;
        }

        public override void OnTileEntered()
        {
            base.OnTileEntered();
            Debug.Log("Character Should not enter ObstracleTile");
        }
    }
}