using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Unity.Game.SaveSystem
{
    [Serializable]
    public class GameData
    {
        // separate best submit from submit history
        public string UserId;
        public SerializableDictionary<GameSession, bool> SessionHistories;
        public SerializableDictionary<long, Submit> SubmitBest;

        //public float musicVolume;
        //public float sfxVolume;

        public GameData()
        {
            SessionHistories = new SerializableDictionary<GameSession, bool>();
            SubmitBest = new SerializableDictionary<long, Submit>();

            // settings
            //this.musicVolume = 80f;
            //this.sfxVolume = 80f;

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