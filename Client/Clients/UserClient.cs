using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Serialization;
using BCrypt.Net;
using Client.Interfaces;
using Client.Services;
using Newtonsoft.Json;

namespace Client.Clients
{
    public class UserClient
    {
        private string _password;
        public string UserName;
        private IHttpService _httpService;
        public string Token { get; private set; }

        public UserClient(IHttpService httpService)
        {
            _httpService = httpService;
        }

        private void SetPassword(string password)
        {
            _password = BCrypt.Net.BCrypt.HashPassword(password);
        }


        public bool CreateUser(string password)
        {
            SetPassword(password);
            HttpContent content = new StringContent(UserToJson());
            HttpResponseMessage httpResponseMessage = _httpService.Post("/user/signup", content);
            return httpResponseMessage.IsSuccessStatusCode;
        }

        public void LoginUser(string password)
        {
            SetPassword(password);
            
            HttpContent content = new StringContent(UserToJson());
            HttpResponseMessage httpResponseMessage = _httpService.Post("/user/login", content);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                Token = httpResponseMessage.Content.ToString();
            }
        }

        private string UserToJson()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                {"username", UserName},
                {"password", _password}
            };
            return JsonConvert.SerializeObject(dict);

        }
    }
}