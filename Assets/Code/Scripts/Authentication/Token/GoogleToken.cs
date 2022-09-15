using Newtonsoft.Json;

namespace Authentication.Token
{
    public class GoogleToken
    {
        [JsonProperty("id_token")]
        public string IdToken { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}