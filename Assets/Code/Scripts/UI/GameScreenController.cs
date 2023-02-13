using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Game.SaveSystem;
using Unity.Game.Level;


namespace Unity.Game.UI
{
    public class GameScreenController : MonoBehaviour
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
            LevelManager.GameWon += OnGameWon;
            SaveManager.GameDataLoaded += OnGameDataLoaded;

            GameScreen.GameRestarted += OnGameRestarted;
            GameScreen.GamePaused += OnGamePaused;
            GameScreen.GameResumed += OnGameResumed;
            GameScreen.GameQuit += OnGameQuit;
        }

        void OnDisable()
        {
            LevelManager.GameWon -= OnGameWon;
            SaveManager.GameDataLoaded -= OnGameDataLoaded;

            GameScreen.GameRestarted -= OnGameRestarted;
            GameScreen.GamePaused -= OnGamePaused;
            GameScreen.GameResumed -= OnGameResumed;
            GameScreen.GameQuit -= OnGameQuit;
        }

        IEnumerator PauseGameTime(float delay = 2f)
        {

            float pauseTime = Time.time + delay;
            float decrement = (delay > 0) ? Time.deltaTime / delay : Time.deltaTime;

            while (Time.timeScale > 0.1f || Time.time < pauseTime)
            {
                Time.timeScale = Mathf.Clamp(Time.timeScale - decrement, 0f, Time.timeScale - decrement);
                yield return null;
            }

            // ramp the timeScale down to 0
            Time.timeScale = 0f;
        }

        void QuitGame()
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
#endif
                SceneManager.LoadSceneAsync(MainMenuSceneName);
        }

        void RestartLevel()
        {

            Time.timeScale = 1f;
#if UNITY_EDITOR
            if (Application.isPlaying)

#endif
                Debug.Log("LoadSceneAsync is triggered");
                SceneManager.LoadSceneAsync(GameSceneName);
                
        }


        void OnGameWon()
        {
            GameWon?.Invoke();
        }

        void OnGamePaused(float delay)
        {
            SettingsLoad?.Invoke();
            StopAllCoroutines();
            StartCoroutine(PauseGameTime(delay));
        }

        void OnGameResumed()
        {
            SettingsUpdated?.Invoke(SettingsData);
            StopAllCoroutines();
            Time.timeScale = 1f;
        }

        void OnGameRestarted()
        {
            RestartLevel();
        }

        void OnGameQuit()
        {
            QuitGame();
        }

        void OnGameDataLoaded(GameData gameData)
        {
            if (gameData == null)
                return;

            SettingsData = gameData;

            SettingsData.musicVolume = gameData.musicVolume;
            SettingsData.sfxVolume = gameData.sfxVolume;

            SettingsUpdated?.Invoke(gameData);

        }

    }
}

