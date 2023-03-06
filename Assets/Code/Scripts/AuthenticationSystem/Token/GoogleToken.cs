using Newtonsoft.Json;
using GoogleAPI;

namespace Authentication.Token
{
    public class GoogleToken
    {
        [JsonProperty("id_token")]
        public string IdToken { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        public GoogleJWTClaim GetJWTClaim()
        {
            string rawJsonClaim = Utility.ReadJwtClaim(IdToken);
            return JsonConvert.DeserializeObject<GoogleJWTClaim>(rawJsonClaim);
        }
    }
}