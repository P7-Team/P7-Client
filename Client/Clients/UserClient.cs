using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using BCrypt.Net;
using Client.Interfaces;
using Newtonsoft.Json;

namespace Client.Clients
{
    public class UserClient
    {
        private string Password { get; set; }
        private string _userName;
        private IHttpService _httpService;
        public string Token { get; private set; }

        public UserClient(IHttpService httpService)
        {
            Token = "";
            _httpService = httpService;
        }

        private void SetPassword(string password)
        {
            Password = password;
        }

        public bool CreateUser(string username, string password)
        {
            SetPassword(password);
            _userName = username;
            HttpContent content = new StringContent(UserToJson());
            HttpResponseMessage httpResponseMessage = _httpService.Post("/user/signup", content);
            return httpResponseMessage.IsSuccessStatusCode;
        }

        public bool LoginUser(string username, string password)
        {
            Password = password;
            _userName = username;
            HttpContent content = new StringContent(UserToJson());
            HttpResponseMessage httpResponseMessage = _httpService.Post("/user/login", content);

            if (!httpResponseMessage.IsSuccessStatusCode) return false;
            Token = httpResponseMessage.Content.ReadAsStringAsync().Result;
            return true;
        }

        private string UserToJson()
        {
            Dictionary<string, string> dict;

            dict = new Dictionary<string, string>()
            {
                {"username", _userName},
                {"password", Password}
            };


            return JsonConvert.SerializeObject(dict);
        }
    }
}