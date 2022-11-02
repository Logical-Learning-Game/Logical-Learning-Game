using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Game.Conditions;

namespace Unity.Game.Map
{
    public class DoorTile : Tile
    {
        [SerializeField] private bool isOpened = false;
        [SerializeField] private DoorTile doorPair;
        public override bool IsEnterable()
        {
            // if door is locked and player is stepped on opposite door, return false
            
            // need implementation later
            return true;
        }

        public override void OnTileEntered()
        {
            base.OnTileEntered();
        }
    }
}