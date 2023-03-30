using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Game.Conditions;
using TMPro;
using Unity.Game.Level;
using Unity.Game.Command;

namespace Unity.Game.MapSystem
{
    public class ConditionTile : Tile
    {
        // Condition, will be implement later
        Condition tileCondition = new Condition();
        [SerializeField] private GameObject conditionDisplay;
        
        //public override bool IsEnterable()
        //{
        //    return true;
        //}

        public override void OnTileEntered()
        {
            base.OnTileEntered();
            if (CommandManager.Instance.isExecuting)
            {
                LevelManager.Instance.SetLastSign(tileCondition.sign);
            }
        }
        
        public void SetTileCondition(ConditionSign sign)
        {
            tileCondition.SetConditionSign(sign);
            //mock setCondition using text
            conditionDisplay.GetComponent<TMP_Text>().text = sign.ToString();
        }
    }
}