using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Game.SaveSystem;
using Unity.Game.Level;


namespace Unity.Game.UI
{
    public class MainMenuScreenController : MonoBehaviour
    {
        public static event Action GameWon;

        public static event Action<GameData> SettingsUpdated;
        public static event Action SettingsLoad;

        [Header("Scenes")]
        [SerializeField] string MainMenuSceneName = "MainMenu";
        [SerializeField] string GameSceneName = "GameMode";

        GameData SettingsData;

        void OnEnable()
        {

        }

        void OnDisable()
        {
 
        }

        void QuitGame()
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
#endif
                SceneManager.LoadSceneAsync(MainMenuSceneName);
        }

    }
}

