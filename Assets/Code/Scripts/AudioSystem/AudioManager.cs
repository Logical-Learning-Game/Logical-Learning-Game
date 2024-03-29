﻿using UnityEngine;
using UnityEngine.Audio;
using System.Linq;
using System;

namespace Unity.Game
{
    public class AudioManager : MonoBehaviour
    {
        // AudioMixerGroup names
        public static string MusicGroup = "Music";
        public static string SfxGroup = "SFX";

        // parameter suffix
        //const string k_Parameter = "Volume";

        [SerializeField] AudioMixer m_MainAudioMixer;

        // basic range of UI sound clips
        [Header("UI Sounds")]
        [Tooltip("General button click.")]
        [SerializeField] AudioClip DefaultButtonSound;
        [Tooltip("General button click.")]
        [SerializeField] AudioClip DefaultHoverSound;
        //[Tooltip("General button click.")]
        //[SerializeField] AudioClip m_AltButtonSound;
        //[Tooltip("General shop purchase clip.")]
        //[SerializeField] AudioClip m_TransactionSound;
        [Tooltip("General error sound.")]
        [SerializeField] AudioClip DefaultWarningSound;


        [Header("Game Sounds")]
        [Tooltip("level win sound.")]
        [SerializeField] AudioClip VictorySound;
        [Tooltip("level start sound.")]
        [SerializeField] AudioClip LevelStartSound;
        [Tooltip("pick item sound.")]
        [SerializeField] AudioClip ItemPickSound;
        [Tooltip("pick command sound.")]
        [SerializeField] AudioClip CommandPickSound;
        [Tooltip("command start sound.")]
        [SerializeField] AudioClip CommandStartSound;
        [Tooltip("character move sound.")]
        [SerializeField] AudioClip CharacterStepSound;
        //[SerializeField] AudioClip m_DefeatSound;
        //[SerializeField] AudioClip m_PotionSound;


        private void Awake()
        {

        }
        void OnEnable()
        {
            //SetVolume(MusicGroup, PlayerPrefs.GetFloat("music", .5f));
            //SetVolume(SfxGroup, PlayerPrefs.GetFloat("sfx", .5f));
        }

        void OnDisable()
        {

        }

        // plays one-shot audio
        public static void PlayOneSFX(AudioClip clip, Vector3 sfxPosition)
        {
            if (clip == null)
                return;

            GameObject sfxInstance = new GameObject(clip.name);
            //Debug.Log($"creating {clip.name} and destory in {clip.length}");
            sfxInstance.transform.position = sfxPosition;

            AudioSource source = sfxInstance.AddComponent<AudioSource>();
            source.clip = clip;

            // set the mixer group (e.g. music, sfx, etc.)
            source.outputAudioMixerGroup = GetAudioMixerGroup(SfxGroup);

            source.Play();

            // destroy after clip length
            Destroy(sfxInstance, clip.length+.2f);
            DontDestroyOnLoad(sfxInstance);
        }

        // return an AudioMixerGroup by name
        public static AudioMixerGroup GetAudioMixerGroup(string groupName)
        {
            AudioManager audioManager = FindObjectOfType<AudioManager>();

            if (audioManager == null)
            {
                //Debug.Log("audioManager is null");
                return null;
            }


            if (audioManager.m_MainAudioMixer == null)
            {
                //Debug.Log("MainAudioMixer is null");
                return null;
            }


            AudioMixerGroup[] groups = audioManager.m_MainAudioMixer.FindMatchingGroups(groupName);

            foreach (AudioMixerGroup match in groups)
            {
                //Debug.Log($"found match: {match.name}:{groupName}");
                if (match.name == groupName)
                    return match;
            }
            //Debug.Log($"cannot find group name:{groupName}");
            return null;

        }
        // convert linear value between 0 and 1 to decibels
        public static float GetDecibelValue(float linearValue)
        {
            // commonly used for linear to decibel conversion
            float conversionFactor = 20f;

            float decibelValue = (linearValue != 0) ? conversionFactor * Mathf.Log10(linearValue) : -144f;
            return decibelValue;
        }

        // convert decibel value to a range between 0 and 1
        public static float GetLinearValue(float decibelValue)
        {
            float conversionFactor = 20f;

            return Mathf.Pow(10f, decibelValue / conversionFactor);

        }

        // converts linear value between 0 and 1 into decibels and sets AudioMixer level
        public static void SetVolume(string groupName, float linearValue)
        {
            AudioManager audioManager = FindObjectOfType<AudioManager>();
            if (audioManager == null)
                return;

            float decibelValue = GetDecibelValue(linearValue);

            if (audioManager.m_MainAudioMixer != null)
            {
                audioManager.m_MainAudioMixer.SetFloat(groupName, decibelValue);
            }
        }

        // returns a value between 0 and 1 based on the AudioMixer's decibel value
        public static float GetVolume(string groupName)
        {

            AudioManager audioManager = FindObjectOfType<AudioManager>();
            if (audioManager == null)
                return 0f;

            float decibelValue = 0f;
            if (audioManager.m_MainAudioMixer != null)
            {
                audioManager.m_MainAudioMixer.GetFloat(groupName, out decibelValue);
            }
            return GetLinearValue(decibelValue);
        }

        // convenient methods for playing a range of pre-defined sounds
        public static void PlayDefaultButtonSound()
        {
            AudioManager audioManager = FindObjectOfType<AudioManager>();
            if (audioManager == null)
                return;

            PlayOneSFX(audioManager.DefaultButtonSound, Vector3.zero);
        }

        public static void PlayDefaultHoverSound()
        {
            AudioManager audioManager = FindObjectOfType<AudioManager>();
            if (audioManager == null)
                return;

            PlayOneSFX(audioManager.DefaultHoverSound, Vector3.zero);
        }

        //public static void PlayDefaultTransactionSound()
        //{
        //    AudioManager audioManager = FindObjectOfType<AudioManager>();
        //    if (audioManager == null)
        //        return;

        //    PlayOneSFX(audioManager.m_TransactionSound, Vector3.zero);
        //}

        public static void PlayDefaultWarningSound()
        {
            AudioManager audioManager = FindObjectOfType<AudioManager>();
            if (audioManager == null)
                return;

            PlayOneSFX(audioManager.DefaultWarningSound, Vector3.zero);
        }
        public static void PlayVictorySound()
        {
            AudioManager audioManager = FindObjectOfType<AudioManager>();
            if (audioManager == null)
                return;

            PlayOneSFX(audioManager.VictorySound, Vector3.zero);
        }

        public static void PlayPickItemSound()
        {
            AudioManager audioManager = FindObjectOfType<AudioManager>();
            if (audioManager == null)
                return;

            PlayOneSFX(audioManager.ItemPickSound, Vector3.zero);
        }

        public static void PlayLevelStartSound()
        {
            AudioManager audioManager = FindObjectOfType<AudioManager>();
            if (audioManager == null)
                return;

            PlayOneSFX(audioManager.LevelStartSound, Vector3.zero);
        }

        public static void PlayCommandPickSound()
        {
            AudioManager audioManager = FindObjectOfType<AudioManager>();
            if (audioManager == null)
                return;

            PlayOneSFX(audioManager.CommandPickSound, Vector3.zero);
        }

        public static void PlayCommandStartSound()
        {
            AudioManager audioManager = FindObjectOfType<AudioManager>();
            if (audioManager == null)
                return;

            PlayOneSFX(audioManager.CommandStartSound, Vector3.zero);
        }
        public static void PlayCharacterStepSound()
        {
            AudioManager audioManager = FindObjectOfType<AudioManager>();
            if (audioManager == null)
                return;

            PlayOneSFX(audioManager.CharacterStepSound, Vector3.zero);
        }

        //public static void PlayDefeatSound()
        //{
        //    AudioManager audioManager = FindObjectOfType<AudioManager>();
        //    if (audioManager == null)
        //        return;

        //    PlayOneSFX(audioManager.m_DefeatSound, Vector3.zero);
        //}

        //public static void PlayPotionDropSound()
        //{
        //    AudioManager audioManager = FindObjectOfType<AudioManager>();
        //    if (audioManager == null)
        //        return;

        //    PlayOneSFX(audioManager.m_PotionSound, Vector3.zero);
        //}

        // event-handling methods
        //void OnSettingsUpdated(GameData GameData)
        //{
        //    // use the GameData to set the music and sfx volume
        //    SetVolume(MusicGroup + k_Parameter, GameData.musicVolume / 100f);
        //    SetVolume(SfxGroup + k_Parameter, GameData.sfxVolume / 100f);
        //}
    }
}
