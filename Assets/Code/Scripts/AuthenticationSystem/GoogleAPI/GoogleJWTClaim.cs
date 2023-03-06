using Newtonsoft.Json;

namespace GoogleAPI
{
    public class GoogleJWTClaim
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }
}