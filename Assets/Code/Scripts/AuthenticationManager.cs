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
using System.Collections;
using UnityEngine.Networking;
using System.Text;
using Repository.GoogleAPI;

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

    private IEnumerator SendSignInLog(GoogleUser user)
    {
        var req = new UnityWebRequest("http://localhost:8000/v1/player/login_log", "POST");

        string body = JsonConvert.SerializeObject(user);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(body);

        req.uploadHandler = new UploadHandlerRaw(bodyRaw);
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");
        yield return req.SendWebRequest();

        if (req.error != null)
        {
            Debug.LogFormat("error {0}", req.error);
        }
    }

    private string ReadJwtClaim(string idToken)
    {
        string[] parts = idToken.Split('.');
        if (parts.Length > 2)
        {
            string decode = parts[1];
            int padLength = 4 - decode.Length % 4;
            if (padLength < 4)
            {
                decode += new string('=', padLength);
            }

            byte[] bytes = System.Convert.FromBase64String(decode);
            string userInfo = Encoding.ASCII.GetString(bytes);
            return userInfo;
        }
        return null;
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

            string rawJsonClaim = ReadJwtClaim(googleToken.IdToken);
            var googleJwtClaim = JsonConvert.DeserializeObject<GoogleJWTClaim>(rawJsonClaim);
            Debug.Log(rawJsonClaim);
            Debug.Log(googleJwtClaim.Email);

            var googleUser = new GoogleUser
            {
                PlayerID = AuthenticationService.Instance.PlayerId,
                Email = googleJwtClaim.Email,
                Name = googleJwtClaim.Name,
            };

            StartCoroutine(SendSignInLog(googleUser));

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
