﻿using System;
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
        public static event Action MainMenuClick;
        public static event Action GoogleSyncClick;
        public static event Action<float> MusicVolumeChanged;
        public static event Action<float> SfxVolumeChanged;

        Slider MusicSlider;
        Slider SFXSlider;

        [SerializeField] VisualElement SettingPanel;

        Button GoogleSyncButton;
        Button QuitGameButton;
        //Button RestartButton;

        GameData gameData;

        private void Awake()
        {
            SetVisualElements();
            RegisterButtonCallbacks();
        }

        void SetVisualElements()
        {
            SettingPanel = GetComponent<PanelScreen>().SettingPanel;
            SFXSlider = SettingPanel.Q<Slider>("SFXSlider");
            MusicSlider = SettingPanel.Q<Slider>("BGMSlider");

            GoogleSyncButton = SettingPanel.Q<Button>("GoogleLinkButton");
            //RestartButton = SettingPanel.Q<Button>("RestartButton");
            QuitGameButton = SettingPanel.Q<Button>("QuitButton");

            ShowVisualElement(GoogleSyncButton, false);
            //if (SceneManager.GetActiveScene().name == "MainMenu")
            //{
            //    //ShowVisualElement(RestartButton, false);
            //    //ShowVisualElement(QuitGameButton, false);
            //    QuitGameButton.Q<Label>().text = "";
            //}
        }

        void RegisterButtonCallbacks()
        {
            SFXSlider?.RegisterValueChangedCallback(ChangeSFXVolume);
            MusicSlider?.RegisterValueChangedCallback(ChangeMusicVolume);

            GoogleSyncButton?.RegisterCallback<ClickEvent>(OnClickGoogleSync);
            //RestartButton?.RegisterCallback<ClickEvent>(OnClickRestart);
            QuitGameButton?.RegisterCallback<ClickEvent>(OnClickQuitGame);

            GoogleSyncButton?.RegisterCallback<MouseOverEvent>(MouseOverButton);
            //RestartButton?.RegisterCallback<MouseOverEvent>(MouseOverButton);
            QuitGameButton?.RegisterCallback<MouseOverEvent>(MouseOverButton);
        }

        void OnClickGoogleSync (ClickEvent evt)
        {
            GoogleSyncClick?.Invoke();
            AudioManager.PlayDefaultButtonSound();
        }
        //void OnClickRestart(ClickEvent evt)
        //{
        //    RestartClick?.Invoke(true);
        //    AudioManager.PlayDefaultButtonSound();
        //}
        void OnClickQuitGame(ClickEvent evt)
        {
            MainMenuClick?.Invoke();
            AudioManager.PlayDefaultButtonSound();
        }

        void MouseOverButton(MouseOverEvent evt)
        {
            AudioManager.PlayDefaultHoverSound();
        }

        public static void ShowVisualElement(VisualElement visualElement, bool state)
        {
            if (visualElement == null)
                return;

            visualElement.style.display = (state) ? DisplayStyle.Flex : DisplayStyle.None;
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
            PanelScreen.OpenSettingPanel -= OnOpenSettingPanel;
            SaveManager.GameDataLoaded -= OnGameDataLoaded;
        }

        public void OnGameDataLoaded(GameData gameData)
        {
            this.gameData = gameData;
            UpdateUserSettingPanel();
            if (gameData.PlayerId == null || gameData.PlayerId == "__guest__")
            {
                ShowVisualElement(GoogleSyncButton, true);
            }
            else
            {
                ShowVisualElement(GoogleSyncButton, false);
            }
        }

        public void OnOpenSettingPanel()
        {
            Debug.Log("invoke opensettingpanel");
            UpdateUserSettingPanel();

            //UpdateUserStat(CalculateUserStat(gameData, WorldDatas));

        }
        void UpdateUserSettingPanel()
        {
            if (SettingPanel == null)
            {
                SettingPanel = GetComponent<PanelScreen>().SettingPanel;
            }

            SettingPanel.Q<Label>("UserIdValue").text = gameData.PlayerId;

            AudioManager.SetVolume(AudioManager.MusicGroup, PlayerPrefs.GetFloat("music", .5f));
            AudioManager.SetVolume(AudioManager.SfxGroup, PlayerPrefs.GetFloat("sfx", .5f));

            MusicSlider.value = AudioManager.GetVolume("Music") * 100;
            SFXSlider.value = AudioManager.GetVolume("SFX") * 100;

        }

        void ChangeSFXVolume(ChangeEvent<float> evt)
        {
            PlayerPrefs.SetFloat("sfx", evt.newValue / 100);
            AudioManager.SetVolume("SFX", evt.newValue / 100);
        }

        void ChangeMusicVolume(ChangeEvent<float> evt)
        {
            PlayerPrefs.SetFloat("music", evt.newValue / 100);
            AudioManager.SetVolume("Music", evt.newValue / 100);
        }


    }
}
