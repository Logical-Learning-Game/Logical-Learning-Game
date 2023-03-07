using Newtonsoft.Json;

namespace Unity.Game.Authentication.GoogleAPI
{
    public class GoogleUserInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("picture")]
        public string PictureURL { get; set; }
    }
}