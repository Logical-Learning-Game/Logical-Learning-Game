using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Game.MapSystem;
using Newtonsoft.Json;

namespace Unity.Game.SaveSystem
{
    [Serializable]
    public class WorldData
    {
        [JsonProperty("world_id")] public long WorldId;
        [JsonProperty("world_name")]public string WorldName;
        [JsonProperty("maps")] public List<Map> MapLists;


        public WorldData(long WorldId, List<Map> MapLists, string WorldName)
        {
            this.WorldId = WorldId;
            this.MapLists = MapLists;
            this.WorldName = WorldName;
        }


    }
}