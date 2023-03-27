using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Unity.Game.Command
{
    public class MaxCommand
    {
        // 0 if disabled , 99 if infinite
        public int Start;
        public int Forward;
        public int Left;
        public int Right;
        public int Back;
        public int Condition;

        public MaxCommand(int start = 1, int forward = 99, int left = 99, int right = 99, int back = 99, int condition = 99)
        {
            Start = start;
            Forward = forward;
            Left = left;
            Right = right;
            Back = back;
            Condition = condition;
        }
    }
}
