using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Net;
using GlobalConfig;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Unity.Game.Authentication.OIDMetadata;
using Unity.Game.Authentication.Token;

namespace Unity.Game.Authentication
{
    public class GoogleAuthenticationManager : MonoBehaviour
    {
        private static GoogleAuthenticationManager instance;

        public static GoogleAuthenticationManager Instance
        {
            get
            {
                if (!instance)
                {
                    Debug.LogError("authentication manager instance should not be null");
                }
                return instance;
            }
        }

        private void Awake()
        {
            instance = this;
        }

        private async void Start()
        {
            await UnityServices.InitializeAsync();
        }

        private async Task<GoogleOIDMetadata> GetGoogleOIDMetaData()
        {

            WebRequest request = WebRequest.Create(GoogleOIDConfig.OID_METADATA_URL);
            request.Headers.Add("Cache-Control", "only-if-cached");

            WebResponse response = await request.GetResponseAsync();
            using var reader = new StreamReader(response.GetResponseStream());
            string jsonText = await reader.ReadToEndAsync();

            GoogleOIDMetadata googleOIDMetadata = JsonConvert.DeserializeObject<GoogleOIDMetadata>(jsonText);
            return googleOIDMetadata;
        }

        public async Task<string> GoogleSignIn()
        {   
            GoogleOIDMetadata googleOIDMetadata = await GetGoogleOIDMetaData();
            var googleOIDCAuth = new GoogleOIDCAuthentication(googleOIDMetadata);
            GoogleToken googleToken = await googleOIDCAuth.GetToken();

            if (AuthenticationService.Instance.SessionTokenExists)
            {
                AuthenticationService.Instance.ClearSessionToken();
            }
            
            await AuthenticationService.Instance.SignInWithOpenIdConnectAsync(GoogleOIDConfig.PROVIDER_NAME, googleToken.IdToken);

            return AuthenticationService.Instance.PlayerId;
        }
    }
}