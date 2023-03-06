using System;
using System.Net.Http;

namespace Unity.Game.SaveSystem
{
    public class APIException : Exception
    {
        public HttpResponseMessage Response { get; }
        public string Content { get; }

        public APIException(string message, HttpResponseMessage response, string content) : base(message)
        {
            Response = response;
            Content = content;
        }

        public APIException(string message, HttpResponseMessage response, string content, Exception innerException) : base(message, innerException)
        {
            Response = response;
            Content = content;
        }
    }
}