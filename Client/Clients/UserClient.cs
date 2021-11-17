using System.Net.Http;
using BCrypt.Net;
using Client.Interfaces;
using Client.Services;

namespace Client.Clients
{
    public class UserClient
    {
        private string _password;
        public string UserName;
        private IHttpService _httpService;

        public UserClient(IHttpService httpService)
        {
            _httpService = httpService;
        }

        private void SetPassword(string password)
        {
            _password = BCrypt.Net.BCrypt.HashPassword(password);
        }

        public void CreateUser(string password)
        {
            SetPassword(password);
            HttpContent content = new StringContent("");
            HttpResponseMessage httpResponseMessage = _httpService.Post("/user/signup",)
            
        }
    }
}