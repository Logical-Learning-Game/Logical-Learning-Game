using GlobalConfig;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Net;

namespace Unity.Game.SaveSystem
{

    public class APIClient
    {
        private HttpClient client;

        public APIClient()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(APIConfig.BASE_URL);
        }

        public async Task<bool> ConnectionCheck()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("/status");
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                Debug.LogErrorFormat("An error occurred while calling the server status check api: {0}", ex);
                return false;
            }
        }

        public async Task SendSessionHistoryData(string playerId, GameSessionHistoryRequest dto)
        {
            string content = JsonConvert.SerializeObject(dto);
            Debug.Log($"content: {content}");
            var requestBody = new StringContent(content, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync($"/v1/players/{playerId}/session_history", requestBody);
            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new APIException("Failed to send session history: The server returned an error", response, errorContent);
            }
        }

        public async Task SendTopSubmitHistory(string playerId, List<TopSubmitHistoryRequest> dto)
        {
            string content = JsonConvert.SerializeObject(dto);
            var requestBody = new StringContent(content, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync($"/v1/players/{playerId}/top_submit_history", requestBody);
            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new APIException("Failed to send top submit history: The server returned an error", response, errorContent);
            }
        }

        public async Task LinkAccount(LinkAccountRequest dto)
        {
            string content = JsonConvert.SerializeObject(dto);
            var requestBody = new StringContent(content, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync($"/v1/players/link_account", requestBody);
            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new APIException("Failed to link account: The server returned an error", response, errorContent);
            }
        }

        public async Task<bool> AccountCheck(string playerId)
        {
            HttpResponseMessage response = await client.GetAsync($"/v1/players/{playerId}");
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return false;
            }
            else
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new APIException("Failed to account check: The server returned an error", response, errorContent);
            }
        }

        public async Task<GameData> GetGameData(string playerId)
        {
            HttpResponseMessage response = await client.GetAsync($"/v1/players/{playerId}/game_data");
            string content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                GameData gameData = JsonConvert.DeserializeObject<GameData>(content);
                return gameData;
            }
            else
            {
                throw new APIException("Failed to get game data: The server returned an error", response, content);
            }
        }

        public async Task<List<WorldData>> GetMapData(string playerId)
        {
            HttpResponseMessage response = await client.GetAsync($"/v1/players/{playerId}/available_maps");
            string content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                List<WorldData> worlds = JsonConvert.DeserializeObject<List<WorldData>>(content);
                return worlds;
            }
            else
            {
                throw new APIException("Failed to get map data: The server returned an error", response, content);
            }
        }
    }
}