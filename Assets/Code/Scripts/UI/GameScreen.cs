using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;
using System.Linq;

namespace Unity.Game.UI
{
    public class GameScreen : MonoBehaviour
    {

        public static event Action<float> GamePaused;
        public static event Action GameResumed;
        public static event Action GameQuit;
        public static event Action GameRestarted;
        public static event Action<float> MusicVolumeChanged;
        public static event Action<float> SfxVolumeChanged;

        [Header("Menu Screen elements")]
        [Tooltip("String IDs to query Visual Elements")]
        [SerializeField] string PauseScreenName = "PauseScreen";
        [SerializeField] string WinScreenName = "GameWinScreen";

        [Header("Blur")]
        [SerializeField] Volume m_Volume;

        GameObject DefaultInGameScreen;

        void Start()
        {

        }

        void OnEnable()
        {
            GameScreenController.GameWon += OnGameWon;
        }

        void OnGameWon()
        {
            StartCoroutine(GameWonRoutine());
        }

        IEnumerator GameWonRoutine()
        {
            yield return new WaitForSeconds(1);

            //// hide the UI
            //m_CharPortraitContainer.style.display = DisplayStyle.None;
            //m_PauseButton.style.display = DisplayStyle.None;

            //AudioManager.PlayVictorySound();
            //ShowVisualElement(m_WinScreen, true);
        }
    }
}