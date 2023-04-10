using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Game.Conditions;

namespace Unity.Game.MapSystem
{
    public class GateTile : Tile
    {
        [SerializeField] private bool isOpened;
        [SerializeField] private GateTile gatePair;
        
        //public override bool IsEnterable()
        //{
        //    // if gate is opened
        //    // if Opposite Site of gatePair can be entered, return true
        //    return true;
        //}

        public override void OnTileEntered()
        {
            base.OnTileEntered();
            Debug.Log("Entered Condition Tile, Character should memorize this pattern");
        }
    }
}