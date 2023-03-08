using System.Collections;
using System.Collections.Generic;
using Unity.Game.SaveSystem;
using UnityEngine;

namespace GlobalConfig
{
    public static class ColorConfig
    {
        public static Dictionary<Medal, Color> MEDAL_COLOR = new Dictionary<Medal, Color>() {
            { Medal.NONE, new Color(1f, 1f, 1f, 0.1f) },
            { Medal.BRONZE,new Color(174/255f,139/255f,85/255f,255/255f) },
            { Medal.SILVER,new Color(180/255f,180/255f,180/255f,255/255f) },
            { Medal.GOLD,new Color(202/255f,177/255f,52/255,255/255f) },
        };
    }
}
