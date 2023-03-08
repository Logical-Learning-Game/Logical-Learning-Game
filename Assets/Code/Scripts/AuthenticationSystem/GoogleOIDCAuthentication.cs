using Unity.Game.Authentication.OIDMetadata;
using GlobalConfig;
using UnityEngine;
using System.Net;
using System.Text;
using System;
using System.Threading.Tasks;
using System.IO;
using Unity.Game.Authentication.Token;
using Newtonsoft.Json;

namespace Unity.Game.Authentication
{
    public class GoogleOIDCAuthentication
    {
        private readonly GoogleOIDMetadata googleOIDMetadata;

        public GoogleOIDCAuthentication(GoogleOIDMetadata googleOIDMetadata)
        {
            this.googleOIDMetadata = googleOIDMetadata;
        }

        public async Task<GoogleToken> GetToken()
        {
            // Generate State and PKCE value
            string state = Utility.RandomBase64URL(32);

            // Creates a redirect URI using an available port on the loopback address.
            string redirectURI = string.Format("http://{0}:{1}/", IPAddress.Loopback, Utility.GetRandomUnusedPort());

            // Creates an HttpListener to listen for requests on that redirect URI.
            var httpListener = new HttpListener();
            httpListener.Prefixes.Add(redirectURI);

            httpListener.Start();

            // Creates the OAuth 2.0 authorization request.
            string authorizationRequest = string.Format("{0}?response_type=code&scope=openid email%20profile&redirect_uri={1}&client_id={2}&state={3}",
                    googleOIDMetadata.AuthorizationEndpoint,
                    Uri.EscapeDataString(redirectURI),
                    GoogleOIDConfig.CLIENT_ID,
                    state
                    );

            // Opens request in the browser.
            System.Diagnostics.Process.Start(authorizationRequest);

            // Waits for the OAuth authorization response.
            var context = await httpListener.GetContextAsync();

            // Sends an HTTP response to the browser.
            var response = context.Response;
            string responseString = string.Format("<html><head><meta http-equiv='refresh' content='10;url=https://google.com'></head><body>Please return to the game.</body></html>");
            var buffer = Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            var responseOutput = response.OutputStream;
            Task responseTask = responseOutput.WriteAsync(buffer, 0, buffer.Length).ContinueWith((task) =>
            {
                responseOutput.Close();
                httpListener.Stop();
            });

            // Checks for errors.
            if (context.Request.QueryString.Get("error") != null)
            {
                Debug.Log(string.Format("OAuth authorization error: {0}.", context.Request.QueryString.Get("error")));
                return default;
            }
            if (context.Request.QueryString.Get("code") == null
                || context.Request.QueryString.Get("state") == null)
            {
                Debug.Log("Malformed authorization response. " + context.Request.QueryString);
                return default;
            }

            // Compares the receieved state to the expected value, to ensure that
            // this app made the request which resulted in authorization.
            string incomingState = context.Request.QueryString.Get("state");
            if (incomingState != state)
            {
                Debug.Log(string.Format("Received request with invalid state ({0})", incomingState));
                return default;
            }

            // extracts the code
            string authCode = context.Request.QueryString.Get("code");

            string googleTokenString = await CodeExchange(authCode, redirectURI);
            return JsonConvert.DeserializeObject<GoogleToken>(googleTokenString);
        }

        private async Task<string> CodeExchange(string authCode, string redirectURI)
        {
            // build the request
            string tokenRequestBody = string.Format("code={0}&redirect_uri={1}&client_id={2}&client_secret={3}&grant_type=authorization_code",
                    authCode,
                    redirectURI,
                    GoogleOIDConfig.CLIENT_ID,
                    GoogleOIDConfig.SECRET
            );


            var tokenRequest = WebRequest.CreateHttp(googleOIDMetadata.TokenEndpoint);
            tokenRequest.Method = "POST";
            tokenRequest.ContentType = "application/x-www-form-urlencoded";
            tokenRequest.Accept = "Accept=text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";

            byte[] byteVersion = Encoding.ASCII.GetBytes(tokenRequestBody);
            tokenRequest.ContentLength = byteVersion.Length;

            var stream = tokenRequest.GetRequestStream();

            await stream.WriteAsync(byteVersion, 0, byteVersion.Length);
            stream.Close();

            try
            {
                // get the response
                WebResponse tokenResponse = await tokenRequest.GetResponseAsync();
                using var reader = new StreamReader(tokenResponse.GetResponseStream());
                // read response body
                string responseText = await reader.ReadToEndAsync();

                return responseText;
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse response = ex.Response as HttpWebResponse;
                    if (response != null)
                    {
                        Debug.LogFormat("HTTP: {0}", response.StatusCode);
                        using (var reader = new StreamReader(response.GetResponseStream()))
                        {
                            string responseText = await reader.ReadToEndAsync();
                            Debug.Log(responseText);
                        }
                    }
                }
            }
            return null;
        }
    }
}
