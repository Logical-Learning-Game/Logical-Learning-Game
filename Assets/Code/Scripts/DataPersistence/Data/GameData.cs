using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Game.SaveSystem
{
    [System.Serializable]
    public class GameData
    {
        // separate best submit from submit history
        public string userId;
        public SerializableDictionary<Submit, bool> SubmitHistory;
        public SerializableDictionary<string, Submit> SubmitBest;

        public float musicVolume;
        public float sfxVolume;

        public GameData(string userId)
        {
            this.userId = userId;
            SubmitHistory = new SerializableDictionary<Submit, bool>();
            SubmitBest = new SerializableDictionary<string, Submit>();

            // settings
            this.musicVolume = 80f;
            this.sfxVolume = 80f;
        }

        public GameData()
        {
            SubmitHistory = new SerializableDictionary<Submit, bool>();
            SubmitBest = new SerializableDictionary<string, Submit>();

            // settings
            this.musicVolume = 80f;
            this.sfxVolume = 80f;

        }

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }

        public void LoadJson(string jsonFilepath)
        {
            JsonUtility.FromJsonOverwrite(jsonFilepath, this);
        }

    }
}