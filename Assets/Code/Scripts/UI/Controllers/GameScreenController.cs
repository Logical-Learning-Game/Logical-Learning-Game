using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Game.SaveSystem;
using Unity.Game.Level;
using Unity.Game.Command;
using Unity.Game.MapSystem;


namespace Unity.Game.UI
{
    public class GameScreenController : MonoBehaviour
    {
        public static event Action<SubmitHistory> GameWon;
        //public static event Action NewMapRestart;
        public static event Action SameMapRestart;
        public static event Action<int> ShowTutorial;
        //public static event Action<GameData> SettingsUpdated;
        //public static event Action SettingsLoad;

        [Header("Scenes")]
        [SerializeField] string MainMenuSceneName = "MainMenu";
        [SerializeField] string GameSceneName = "GameMode";

        GameData gameData;


        void OnEnable()
        {
            LevelManager.GameWon += OnGameWon;
            SaveManager.GameDataLoaded += OnGameDataLoaded;

            GameScreen.GameRestarted += OnGameRestarted;
            GameScreen.GamePaused += OnGamePaused;
            GameScreen.GameResumed += OnGameResumed;
            GameScreen.GameQuit += OnGameQuit;

            LevelManager.OnMapEnter += OnMapEntered;
        }

        void OnDisable()
        {
            LevelManager.GameWon -= OnGameWon;
            SaveManager.GameDataLoaded -= OnGameDataLoaded;

            GameScreen.GameRestarted -= OnGameRestarted;
            GameScreen.GamePaused -= OnGamePaused;
            GameScreen.GameResumed -= OnGameResumed;
            GameScreen.GameQuit -= OnGameQuit;

            LevelManager.OnMapEnter -= OnMapEntered;
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

        void RestartLevel(bool isSameMap)
        {
            Time.timeScale = 1f;
#if UNITY_EDITOR
            if (Application.isPlaying)
#endif
                if (!isSameMap)
                {
                    //NewMapRestart?.Invoke();
                    SceneManager.LoadSceneAsync(GameSceneName);
                }
                else
                {
                    OnGameResumed();
                    SameMapRestart?.Invoke();
                }


        }

        void OnGameWon(SubmitHistory submit)
        {
            GameWon?.Invoke(submit);
        }

        void OnGamePaused(float delay)
        {
            //SettingsLoad?.Invoke();
            StopAllCoroutines();
            StartCoroutine(PauseGameTime(delay));
        }

        void OnGameResumed()
        {
            //SettingsUpdated?.Invoke(SettingsData);
            StopAllCoroutines();
            Time.timeScale = 1f;
        }

        void OnGameRestarted(bool isSameMap)
        {
            Debug.Log("OnGameRestarted invoked");
            RestartLevel(isSameMap);
        }

        void OnGameQuit()
        {
            QuitGame();
        }

        void OnMapEntered(Map map)
        {
            if(!gameData.SubmitBest.ContainsKey(map.Id))
            {
                
                int contentIndex = map.Id switch
                {
                    0 => 0,
                    1 => 1,
                    2 => 2,
                    _ => -1,
                };
                if(contentIndex != -1)
                {
                    OnGamePaused(.5f);
                    ShowTutorial?.Invoke(contentIndex);
                }
            }
        }

        void OnGameDataLoaded(GameData gameData)
        {
            if (gameData == null)
                return;

            this.gameData = gameData;


        }

    }
}

