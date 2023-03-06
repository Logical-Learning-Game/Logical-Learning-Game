using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GlobalConfig;
using Unity.Game.MapSystem;
using Unity.Game.UI;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEngine.SceneManagement;
using Unity.Game.Level;
using Unity.Game.SaveSystem;
using Unity.Game.RuleSystem;

namespace Unity.Game.UI
{
    public class SettingPanelManager : MonoBehaviour
    {
        public static event Action<float> MusicVolumeChanged;
        public static event Action<float> SfxVolumeChanged;

        Slider MusicSlider;
        Slider SFXSlider;
        

        Button GoogleSyncButton;
        Button QuitGameButton;
        Button RestartButton;

        GameData gameData;

        private void Awake()
        {
            SetVisualElements();
            RegisterButtonCallbacks();
        }

        void SetVisualElements()
        {
            VisualElement SettingPanel = GetComponent<PanelScreen>().SettingPanel;
            SFXSlider = SettingPanel.Q<Slider>("SFXSlider");
            MusicSlider = SettingPanel.Q<Slider>("BGMSlider");

            GoogleSyncButton = SettingPanel.Q<Button>("GoogleLinkButton");
            RestartButton = SettingPanel.Q<Button>("RestartButton");
            QuitGameButton = SettingPanel.Q<Button>("QuitButton");
        }
        void RegisterButtonCallbacks()
        {
            SFXSlider?.RegisterValueChangedCallback(ChangeSFXVolume);
            MusicSlider?.RegisterValueChangedCallback(ChangeMusicVolume);
        }
        private void Start()
        {
            //MusicVolume = PlayerPrefs.GetFloat("musicvolume",50);
            //SFXVolume = PlayerPrefs.GetFloat("sfxvolume", 50);
        }

        private void OnEnable()
        {
            PanelScreen.OpenSettingPanel += OnOpenSettingPanel;
            SaveManager.GameDataLoaded += OnGameDataLoaded;
        }

        private void OnDisable()
        {
            PanelScreen.OpenStatPanel -= OnOpenSettingPanel;
            SaveManager.GameDataLoaded -= OnGameDataLoaded;
        }

        public void OnGameDataLoaded(GameData gameData)
        {
            this.gameData = gameData;
        }

        public void OnOpenSettingPanel()
        {
            UpdateUserSettingPanel();

            //UpdateUserStat(CalculateUserStat(gameData, WorldDatas));

        }
        void UpdateUserSettingPanel()
        {
            VisualElement SettingPanel = GetComponent<PanelScreen>().SettingPanel;
            SettingPanel.Q<Label>("UserIdValue").text = gameData.UserId;

            Debug.Log(AudioManager.GetVolume("Music"));
            MusicSlider.value = AudioManager.GetVolume("Music")*100;
            SFXSlider.value = AudioManager.GetVolume("SFX")*100;

        }

        void ChangeSFXVolume(ChangeEvent<float> evt)
        {
            PlayerPrefs.SetFloat("sfx", evt.newValue / 100);
            AudioManager.SetVolume("SFX",evt.newValue / 100);
        }

        void ChangeMusicVolume(ChangeEvent<float> evt)
        {
            PlayerPrefs.SetFloat("music", evt.newValue / 100);
            AudioManager.SetVolume("Music", evt.newValue / 100);
        }


    }
}
