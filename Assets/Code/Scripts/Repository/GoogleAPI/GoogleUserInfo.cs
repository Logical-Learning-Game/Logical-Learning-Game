using Newtonsoft.Json;

namespace Repository.GoogleAPI
{
    public class GoogleUserInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("picture")]
        public string PictureURL { get; set; }
    }
}