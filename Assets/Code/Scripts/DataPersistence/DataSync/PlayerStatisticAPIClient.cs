using Newtonsoft.Json;
using System.Net.Http;
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
        }

        public async Task<HttpResponseMessage> ConnectionCheck()
        {
            return null;
        }

        public async Task<HttpResponseMessage> SendSessionHistoryData(string playerId, GameSessionHistoryRequestDto dto)
        {
            string content = JsonConvert.SerializeObject(dto);
            Debug.Log($"content: {content}");
            var requestBody = new StringContent(content);
            var response = await client.PostAsync($"http://localhost:8000/v1/players/{playerId}/statistics", requestBody);
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