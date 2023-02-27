using GlobalConfig;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
                Debug.LogErrorFormat("An error occurred while calling the server status check api: {}", ex);
                return false;
            }
        }

        public async Task<HttpResponseMessage> SendSessionHistoryData(string playerId, GameSessionHistoryRequestDTO dto)
        {
            string content = JsonConvert.SerializeObject(dto);
            var requestBody = new StringContent(content, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync($"/v1/players/{playerId}/statistics", requestBody);
            return response;       
        }

        public void SendSubmitBest()
        {

        }

        public void GetSessionHistoryData()
        {

        }

        public async Task<List<WorldResponseDTO>> GetMapData(string playerId)
        {
            HttpResponseMessage response = await client.GetAsync($"v1/players/{playerId}/available_maps");
            string content = await response.Content.ReadAsStringAsync();
            List<WorldResponseDTO> dto = JsonConvert.DeserializeObject<List<WorldResponseDTO>>(content);
            return dto;
        }
    }
}