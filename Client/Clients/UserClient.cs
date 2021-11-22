using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using BCrypt.Net;
using Client.Interfaces;
using Newtonsoft.Json;

namespace Client.Clients
{
    public class UserClient : IUserClient
    {
        private IHttpService _httpService;
        public string Token { get; private set; }

        public UserClient(IHttpService httpService)
        {
            Token = "";
            _httpService = httpService;
        }


        public bool CreateUser(string username, string password)
        {
            HttpContent content = new StringContent(UserToJson(username,password));
            HttpResponseMessage httpResponseMessage = _httpService.Post("/user/signup", content);
            return httpResponseMessage.IsSuccessStatusCode;
        }

        public bool LoginUser(string username, string password)
        {
            HttpContent content = new StringContent(UserToJson(username,password));
            HttpResponseMessage httpResponseMessage = _httpService.Post("/user/login", content);

            if (!httpResponseMessage.IsSuccessStatusCode) return false;
            Token = httpResponseMessage.Content.ReadAsStringAsync().Result;
            return true;
        }

        private string UserToJson(string username, string password)
        {
            Dictionary<string, string> dict;

            dict = new Dictionary<string, string>()
            {
                {"username", username},
                {"password", password}
            };


            return JsonConvert.SerializeObject(dict);
        }
    }
}