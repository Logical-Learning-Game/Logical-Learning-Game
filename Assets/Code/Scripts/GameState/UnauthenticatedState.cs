using Newtonsoft.Json;
using Repository.GoogleAPI;
using System.IO;
using System.Net;
using TMPro;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

namespace State
{
    public class UnauthenticatedState : AbstractGameState
    {
        public override async void Authenticated()
        {
            GameStateManager.Instance.LoginPanel.SetActive(false);
            GameStateManager.Instance.ProfilePanel.SetActive(true);

           
            string userAccessToken = PlayerPrefs.GetString("access_token");

            GameObject profilePanel = GameStateManager.Instance.ProfilePanel;

            GameObject name = profilePanel.transform.Find("Name").gameObject;
            GameObject profileImage = profilePanel.transform.Find("ProfileImage").gameObject;

            var nameText = name.GetComponent<TMP_Text>();
            var image = profileImage.GetComponent<Image>();

            PlayerInfo playerInfo = AuthenticationService.Instance.PlayerInfo;

            if (userAccessToken != null && userAccessToken.Length > 0)
            {
                string userInfoEndpoint = AuthenticationManager.Instance.GoogleOIDMetadata.UserInfoEndpoint;

                HttpWebRequest request = WebRequest.CreateHttp(userInfoEndpoint);
                request.Method = "GET";
                request.Headers.Add(string.Format("Authorization: Bearer {0}", userAccessToken));
                request.ContentType = "application/x-www-form-urlencoded";
                request.Accept = "Accept=text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";

                WebResponse response = await request.GetResponseAsync();

                using var reader = new StreamReader(response.GetResponseStream());
                string responseText = await reader.ReadToEndAsync();

                var googleUserInfo = JsonConvert.DeserializeObject<GoogleUserInfo>(responseText);

                nameText.text = googleUserInfo.Name;
                GameStateManager.Instance.StartCoroutine(DownloadImage(googleUserInfo.PictureURL, image));
            }
            else
            {
                nameText.text = "Guest";
                Sprite userPlaceholder = GameStateManager.Instance.UserPlaceholder;

                image.sprite = userPlaceholder;
            }

            GameStateManager.Instance.ChangeState(new AuthenticatedState());
        }

        private IEnumerator DownloadImage(string url, Image image)
        {
            using var www = UnityWebRequestTexture.GetTexture(url);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ProtocolError || www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogErrorFormat("error while receiving {0}", www.error);
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                image.sprite = sprite;
            }
        }
    }
}