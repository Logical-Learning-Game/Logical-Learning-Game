using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Unity.Game.SaveSystem
{
    [Serializable]
    public class GameData
    {
        // separate best submit from submit history
        [JsonProperty("player_id")] public string PlayerId;

        [JsonProperty("session_histories")] public List<SessionStatus> SessionHistories;
        [JsonProperty("top_submits")] public Dictionary<long, SubmitHistory> SubmitBest;

        public GameData()
        {
            SessionHistories = new List<SessionStatus>();
            SubmitBest = new Dictionary<long, SubmitHistory>();
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static GameData LoadJson(string jsonString)
        {

            try
            {
                var gameData = JsonConvert.DeserializeObject<GameData>(jsonString);

                return gameData;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load game data: {e.Message}");
                return null;
            }
        }

        public int GetCurrentStar()
        {
            int starCount = 0;

            foreach (SubmitHistory submitHistory in SubmitBest.Values)
            {
                if (submitHistory.IsCompleted)
                {
                    foreach (RuleHistory ruleHistory in submitHistory.RuleHistories)
                    {
                        if (ruleHistory.IsPass)
                        {
                            starCount++;
                        }
                    }
                }
            }

            return starCount;
        }

    }
}