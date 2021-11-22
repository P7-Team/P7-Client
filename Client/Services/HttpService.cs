using System;
using System.Net.Http;
using System.Threading.Tasks;
using Client.Interfaces;
using Client.Models;

namespace Client.Services
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _client;

        private string _ip = "";

        public HttpService(string ip, string token = "")
        {
            _client = new HttpClient();
            _ip = ip;
            _client.BaseAddress = new Uri(ip);
            if (token == "") return;
            _client.DefaultRequestHeaders.Add("Authorization", token);
        }

        public void SetToken(string token)
        {
            _client.DefaultRequestHeaders.Add("Authorization", token);
        }

        public HttpResponseMessage Send(HttpRequestMessage message)
        {
            return _client.SendAsync(message).Result;
        }

        public HttpResponseMessage Get(string uri)
        {
            return _client.GetAsync(new Uri(_ip + uri)).Result;
        }

        public HttpResponseMessage Post(string uri, HttpContent content)
        {
            return _client.PostAsync(new Uri(_ip + uri), content).Result;
        }

        public HttpResponseMessage Delete(string uri)
        {
            return _client.DeleteAsync(new Uri(_ip + uri)).Result;
        }
    }
}