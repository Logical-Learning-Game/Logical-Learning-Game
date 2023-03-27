using System.Collections;
using System.Collections.Generic;
using Unity.Game.SaveSystem;
using UnityEngine;

namespace GlobalConfig
{
    public static class ColorConfig
    {
        public static Dictionary<Medal, Color> MEDAL_COLOR = new Dictionary<Medal, Color>() {
            { Medal.NONE, new Color(1f, 1f, 1f, 0f) },
            { Medal.BRONZE,new Color(130/255f,105/255f,72/255f,255/255f) },
            { Medal.SILVER,new Color(144/255f,142/255f,150/255f,255/255f) },
            { Medal.GOLD,new Color(166/255f,143/255f,17/255,255/255f) },
        };

        public static Color DISABLED = new Color(.5f,.5f,.5f);
        public static Color ENABLED = new Color(1f, 1f, 1f);
    }
}
