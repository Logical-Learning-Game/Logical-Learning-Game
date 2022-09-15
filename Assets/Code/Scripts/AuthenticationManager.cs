using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Authentication.OIDMetadata;
using System.Net;
using GlobalConfig;
using System.IO;
using Newtonsoft.Json;
using Authentication;
using Authentication.Token;

public class AuthenticationManager : MonoBehaviour
{
    private static AuthenticationManager _instance;

    public static AuthenticationManager Instance
    {
        get
        {
            if (!_instance)
            {
                Debug.LogError("authentication manager instance should not be null");
            }
            return _instance;
        }
    }

    public GoogleOIDMetadata GoogleOIDMetadata { get; private set; }

    private void Awake()
    {
        _instance = this;
    }

    private async void Start()
    {
        await UnityServices.InitializeAsync();
    }

    public async void OnAnonymousSignInClick()
    {
        PlayerPrefs.DeleteAll();

        if (AuthenticationService.Instance.SessionTokenExists)
        {
            AuthenticationService.Instance.ClearSessionToken();
        }

        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            GameStateManager.Instance.Authenticated();
        }
        catch (AuthenticationException ex)
        {
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
        }
    }

    public async void OnGoogleSignInClick()
    {
        try
        {
            WebRequest request = WebRequest.Create(GoogleOIDConfig.OID_METADATA_URL);
            request.Headers.Add("Cache-Control", "only-if-cached");

            WebResponse response = await request.GetResponseAsync();
            using var reader = new StreamReader(response.GetResponseStream());
            string jsonText = await reader.ReadToEndAsync();

            GoogleOIDMetadata = JsonConvert.DeserializeObject<GoogleOIDMetadata>(jsonText);
        }
        catch (WebException ex)
        {
            Debug.LogErrorFormat("cannot get google openid metadata", ex);
            return;
        }

        try
        {
            var googleOIDCAuth = new GoogleOIDCAuthentication(GoogleOIDMetadata);
            await googleOIDCAuth.Start();

            string tokenString = googleOIDCAuth.TokenString;
            var googleToken = JsonConvert.DeserializeObject<GoogleToken>(tokenString);

            await AuthenticationService.Instance.SignInWithOpenIdConnectAsync(GoogleOIDConfig.PROVIDER_NAME, googleToken.IdToken);
            PlayerPrefs.SetString("access_token", googleToken.AccessToken);

            GameStateManager.Instance.Authenticated();
        }
        catch (WebException ex)
        {
            Debug.LogException(ex);
        }
        catch (AuthenticationException ex)
        {
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
        }
    }
}
