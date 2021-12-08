using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using BCrypt.Net;
using Client.Interfaces;
using Newtonsoft.Json;

namespace Client.Clients
{
    public class UserClient : IUserClient
    {
        private IHttpService _httpService;


        public UserClient(IHttpService httpService)
        {
            _httpService = httpService;
        }


        public bool CreateUser(string username, string password)
        {
            HttpContent content = new StringContent(UserToJson(username, password), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = _httpService.Post("api/user/signup", content);
            return httpResponseMessage.IsSuccessStatusCode;
        }

        public string LoginUser(string username, string password)
        {
            HttpContent content = new StringContent(UserToJson(username, password), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = _httpService.Post("api/user/login", content);

            // TODO: Use the username in the UI
            return !httpResponseMessage.IsSuccessStatusCode
                ? ""
                : JsonConvert.DeserializeObject<Dictionary<string, string>>(httpResponseMessage.Content.ReadAsStringAsync().Result)["token"] ;
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