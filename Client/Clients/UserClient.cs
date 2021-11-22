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
        private IHttpService _httpService;


        public UserClient(IHttpService httpService)
        {
            _httpService = httpService;
        }


        public bool CreateUser(string username, string password)
        {
            HttpContent content = new StringContent(UserToJson(username, password));
            HttpResponseMessage httpResponseMessage = _httpService.Post("/user/signup", content);
            return httpResponseMessage.IsSuccessStatusCode;
        }

        public string LoginUser(string username, string password)
        {
            HttpContent content = new StringContent(UserToJson(username, password));
            HttpResponseMessage httpResponseMessage = _httpService.Post("/user/login", content);

            return !httpResponseMessage.IsSuccessStatusCode ? "" : httpResponseMessage.Content.ReadAsStringAsync().Result;
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