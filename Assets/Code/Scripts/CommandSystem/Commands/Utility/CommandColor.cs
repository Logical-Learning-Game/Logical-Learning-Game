using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Game.Command
{
    public class CommandColor {

        public static Color Default = new Color(0.5f, 0.5f, 0.5f);
        public static Color Error = new Color(1f, 0f, 0f);
        public static Color Success = new Color(0f, 1f, 0f);
        public static Color Warning = new Color(1f, 1f, 0f);
        public static Color Selected = new Color(0f, 0f, 1f);
        public static Color Unselected = new Color(0.5f, 0.5f, 0.5f);
 
    }
}