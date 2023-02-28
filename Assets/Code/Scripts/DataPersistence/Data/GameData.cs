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
        [JsonProperty("user_id")] public string UserId;
        [JsonProperty("session_histories")] public SerializableDictionary<GameSession, bool> SessionHistories;
        [JsonProperty("submit_best")] public SerializableDictionary<long, SubmitHistory> SubmitBest;

        public GameData()
        {
            SessionHistories = new SerializableDictionary<GameSession, bool>();
            SubmitBest = new SerializableDictionary<long, SubmitHistory>();

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