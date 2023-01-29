using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Game.SaveSystem
{
    public class GameData
    {
        // separate best submit from submit history
        public string userId;
        Dictionary<Submit,bool> SubmitHistory;
        Dictionary<string, List<Submit>> SubmitBest;
    }

}