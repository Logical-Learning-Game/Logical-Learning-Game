using UnityEngine;
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
        const string k_Parameter = "Volume";

        [SerializeField] AudioMixer m_MainAudioMixer;

        // basic range of UI sound clips
        [Header("UI Sounds")]
        [Tooltip("General button click.")]
        [SerializeField] AudioClip DefaultButtonSound;
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

        void OnEnable()
        {
            //SettingsScreen.SettingsUpdated += OnSettingsUpdated;
            //GameScreenController.SettingsUpdated += OnSettingsUpdated;
        }

        void OnDisable()
        {
            //SettingsScreen.SettingsUpdated -= OnSettingsUpdated;
            //GameScreenController.SettingsUpdated -= OnSettingsUpdated;
        }

        // plays one-shot audio
        public static void PlayOneSFX(AudioClip clip, Vector3 sfxPosition)
        {
            if (clip == null)
                return;

            GameObject sfxInstance = new GameObject(clip.name);
            sfxInstance.transform.position = sfxPosition;

            AudioSource source = sfxInstance.AddComponent<AudioSource>();
            source.clip = clip;
            source.Play();

            // set the mixer group (e.g. music, sfx, etc.)
            source.outputAudioMixerGroup = GetAudioMixerGroup(SfxGroup);

            // destroy after clip length
            Destroy(sfxInstance, clip.length);
        }

        // return an AudioMixerGroup by name
        public static AudioMixerGroup GetAudioMixerGroup(string groupName)
        {
            AudioManager audioManager = FindObjectOfType<AudioManager>();

            if (audioManager == null)
                return null;

            if (audioManager.m_MainAudioMixer == null)
                return null;

            AudioMixerGroup[] groups = audioManager.m_MainAudioMixer.FindMatchingGroups(groupName);

            foreach (AudioMixerGroup match in groups)
            {
                if (match.ToString() == groupName)
                    return match;
            }
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

        //public static void PlayAltButtonSound()
        //{
        //    AudioManager audioManager = FindObjectOfType<AudioManager>();
        //    if (audioManager == null)
        //        return;

        //    PlayOneSFX(audioManager.m_AltButtonSound, Vector3.zero);
        //}

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
        //void OnSettingsUpdated(GameData gameData)
        //{
        //    // use the gameData to set the music and sfx volume
        //    SetVolume(MusicGroup + k_Parameter, gameData.musicVolume / 100f);
        //    SetVolume(SfxGroup + k_Parameter, gameData.sfxVolume / 100f);
        //}
    }
}
