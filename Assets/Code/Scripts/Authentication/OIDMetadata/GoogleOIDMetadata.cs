using Newtonsoft.Json;

namespace Authentication.OIDMetadata
{
    public class GoogleOIDMetadata
    {

        [JsonProperty("authorization_endpoint")]
        public string AuthorizationEndpoint { get; set; }

        [JsonProperty("token_endpoint")]
        public string TokenEndpoint { get; set; }

        [JsonProperty("userinfo_endpoint")]
        public string UserInfoEndpoint { get; set; }
    }
}