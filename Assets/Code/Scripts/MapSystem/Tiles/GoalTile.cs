using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Game.Command;

namespace Unity.Game.MapSystem
{
    public class GoalTile : Tile
    {

        //public override bool IsEnterable()
        //{
        //    return true;
        //}

        public override void OnTileEntered()
        {
            base.OnTileEntered();
            Debug.Log("Enter goal!, this level should be pass");
            CommandManager.Instance.SetStopOnNextAction(true);
        }
    }
}