using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Game.Conditions;
using TMPro;

namespace Unity.Game.MapSystem
{
    public class ConditionTile : Tile
    {
        // Condition, will be implement later
        Condition tileCondition = new Condition();
        [SerializeField] private GameObject conditionDisplay;
        public override bool IsEnterable()
        {
            return true;
        }

        public override void OnTileEntered()
        {
            base.OnTileEntered();
            Player.Instance.SetLastSign(tileCondition.sign);
            Debug.Log("Entered Condition Tile, Character should memorize this pattern");
        }
        
        public void SetTileCondition(ConditionSign sign)
        {
            tileCondition.SetConditionSign(sign);
            //mock setCondition using text
            conditionDisplay.GetComponent<TMP_Text>().text = sign.ToString();
        }
    }
}