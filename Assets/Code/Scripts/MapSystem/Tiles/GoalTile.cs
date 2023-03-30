using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Game.Command;
using Unity.Game.Level;

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
            if (CommandManager.Instance.isExecuting)
            {
                Debug.Log("Enter goal!, this level should be pass");
                LevelManager.Instance.SetIsPlayerReachGoal();
                CommandManager.Instance.SetStopOnNextAction(true);
            }
            
        }
    }
}