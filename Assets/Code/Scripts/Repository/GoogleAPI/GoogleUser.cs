
using Newtonsoft.Json;

namespace Repository.GoogleAPI
{
    public class GoogleUser
    {
        [JsonProperty("player_id")]
        public string PlayerID { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}