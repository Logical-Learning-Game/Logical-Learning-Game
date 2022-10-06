using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace Unity.Game.Command
{
    public class CommandBarManager : MonoBehaviour
    {
        public static CommandBarManager Instance { get; private set; }

        private HorizontalLayoutGroup layoutGroup;
        private float spacing;
        private void Awake()
        {
            layoutGroup = GetComponent<HorizontalLayoutGroup>();
        }

        private float CalculateSpacing()
        {
            return 4f;
        }

        private void Update()
        {
            layoutGroup.spacing = CalculateSpacing();
        }
    }
}
