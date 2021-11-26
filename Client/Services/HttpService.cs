using System;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Client.Interfaces;
using Client.Models;

namespace Client.Services
{
    public class HttpService : IHttpService
    {
        private static HttpService _instance;
        
        private readonly HttpClient _client;
        private readonly string _ip;

        public static HttpService GetHttpService(string ip = "", string token = "")
        {
            if (_instance == null)
            {
                if (ip == null)
                {
                    throw new ArgumentException("IP was empty");
                }
                _instance = new HttpService(ip, token);
            }

            return _instance;
        }
        

        protected HttpService(string ip, string token = "")
        {
            _client = new HttpClient();
            _ip = ip;
            _client.BaseAddress = new Uri(ip);
            if (token == "") return;
            _client.DefaultRequestHeaders.Add("Authorization", token);
        }

        /// <summary>
        /// Sets the token for which the HTTP client should use to verify the user.
        /// </summary>
        /// <param name="token">The token which should be used to verify the user.</param>
        public void SetToken(string token)
        {
            _client.DefaultRequestHeaders.Add("Authorization", token);
        }

        /// <summary>
        /// Generic HTTP send message, can be used for any HTTP request type.
        /// </summary>
        /// <param name="message">The message that should be sent, defined as HttpRequestMessage</param>
        /// <returns>Success status of the request.</returns>
        public HttpResponseMessage Send(HttpRequestMessage message)
        {
            return _client.SendAsync(message).Result;
        }

        /// <summary>
        /// Get HTTP request.
        /// </summary>
        /// <param name="uri">The URI of the endpoint which the request should be sent to.</param>
        /// <returns>Success status of the request.</returns>
        public HttpResponseMessage Get(string uri)
        {
            return _client.GetAsync(new Uri(_ip + uri)).Result;
        }

        /// <summary>
        /// Post HTTP request.
        /// </summary>
        /// <param name="uri">The URI of the endpoint which the request should be sent to.</param>
        /// <param name="content">The content of the Post request.</param>
        /// <returns>Success status of the request.</returns>
        public HttpResponseMessage Post(string uri, HttpContent content)
        {
            return _client.PostAsync(new Uri(_ip + uri), content).Result;
        }
        
        /// <summary>
        /// Delete HTTP request.
        /// </summary>
        /// <param name="uri">The URI of the endpoint which the request should be sent to.</param>
        /// <returns>Success status of the request.</returns>
        public HttpResponseMessage Delete(string uri)
        {
            return _client.DeleteAsync(new Uri(_ip + uri)).Result;
        }
    }
}