using Newtonsoft.Json;

namespace Unity.Game.SaveSystem
{
    public class LinkAccountRequest
    {
        [JsonProperty("player_id")]
        public string PlayerId { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
