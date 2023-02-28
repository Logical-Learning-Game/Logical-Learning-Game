using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System;
using Unity.Game.SaveSystem;

namespace Unity.Game.UI
{
    public class IntroScreenController : MonoBehaviour
    {
        // events
        public static event Action HideContinueButton;
        //public static event Action<LevelSO> ShowLevelInfo;
        //public static event Action<List<ChatSO>> ShowChats;
        //public static event Action MainMenuExited;


        void Awake()
        {
            //m_ChatData?.AddRange(Resources.LoadAll<ChatSO>(m_ChatResourcePath));
        }

        void OnEnable()
        {
            SaveManager.GameDataLoaded += OnGameDataLoaded;
        }

        void OnDisable()
        {
            //HomeScreen.PlayButtonClicked -= OnPlayGameLevel;
        }

        void Start()
        {

        }

        void OnGameDataLoaded(GameData gameData)
        {
            // if loaded gamedata is new game, the screen will hide continue button
            if (gameData.UserId == "")
            {
                HideContinueButton?.Invoke();
            }
        }

    }
}