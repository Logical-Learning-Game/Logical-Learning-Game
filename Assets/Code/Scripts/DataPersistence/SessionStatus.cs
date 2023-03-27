using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using Unity.Game.Command;
using Unity.Game.Level;
using Unity.Game.MapSystem;
using UnityEngine;
using Newtonsoft.Json;

namespace Unity.Game.SaveSystem
{
    [Serializable]
    public class SessionStatus
    {
        [JsonProperty("session")] public GameSession Session;
        [JsonProperty("status")] public bool Status;

        public SessionStatus(GameSession gameSession, bool sessionStatus)
        {
            this.Session = gameSession;
            this.Status = sessionStatus;
        }
    }
}
