using GlobalConfig;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Unity.Game.SaveSystem
{
    public class PlayerStatisticAPIClient
    {
        private HttpClient client;

        public PlayerStatisticAPIClient()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(APIConfig.BASE_URL);
        }

        public async Task<bool> ConnectionCheck()
        {
            try
            {
                var response = await client.GetAsync("/status");
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                Debug.LogErrorFormat("An error occurred while calling the server status check api: {}", ex);
                return false;
            }
        }

        public async Task<HttpResponseMessage> SendSessionHistoryData(string playerId, GameSessionHistoryRequestDto dto)
        {
            string content = JsonConvert.SerializeObject(dto);
            var requestBody = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"/v1/players/{playerId}/statistics", requestBody);
            return response;       
        }

        public void SendSubmitBest()
        {

        }

        public void GetSessionHistoryData()
        {

        }

        public void GetMapData()
        {

        }
    }
}