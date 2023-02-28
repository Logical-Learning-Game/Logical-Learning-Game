using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

namespace Unity.Game.SaveSystem
{
    [Serializable]
    public class GameData
    {
        // separate best submit from submit history
        public string UserId;
        public SerializableDictionary<GameSession, bool> SessionHistories;
        public SerializableDictionary<long, SubmitHistory> SubmitBest;

        //public float musicVolume;
        //public float sfxVolume;

        public GameData()
        {
            SessionHistories = new SerializableDictionary<GameSession, bool>();
            SubmitBest = new SerializableDictionary<long, SubmitHistory>();

            // settings
            //this.musicVolume = 80f;
            //this.sfxVolume = 80f;

        }

        public string ToJson()
        {
            //return JsonUtility.ToJson(this);
            return JsonConvert.SerializeObject(this);
        }

        public static GameData LoadJson(string jsonString)
        {
            //JsonUtility.FromJsonOverwrite(jsonFilepath, this);
            return JsonConvert.DeserializeObject<GameData>(jsonString);
        }

    }
}