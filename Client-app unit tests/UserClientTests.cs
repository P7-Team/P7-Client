using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Client.Clients;
using Client.Interfaces;
using Client.Services;
using Newtonsoft.Json;
using Xunit;

namespace Client_app
{
    class HttpClientTester : IHttpService
    {
        public HttpResponseMessage Send(HttpRequestMessage message)
        {
            throw new System.NotImplementedException();
        }

        public HttpResponseMessage Get(string uri)
        {
            throw new System.NotImplementedException();
        }

        public HttpResponseMessage Post(string uri, HttpContent content)
        {
            string output = ContentReader.ReadStreamContent(content.);
            Dictionary<string, string> dictionary =
                JsonConvert.DeserializeObject<Dictionary<string, string>>(content.ToString());
            string username = "username";
            string password = BCrypt.Net.BCrypt.HashPassword("password");
            if (uri.EndsWith("login"))
            {
                if (username == dictionary["username"] && dictionary["password"]== password)
                {
                    return new HttpResponseMessage(HttpStatusCode.Accepted);
                }
            }

            throw new System.NotImplementedException();
        }

        public HttpResponseMessage Delete(string uri)
        {
            throw new System.NotImplementedException();
        }
    }

    public class UserClientTests
    {
        [Fact]
        public void CreateUser_Returns_201()
        {
            UserClient userClient = new UserClient(new HttpClientTester());
        }

        [Fact]
        public void CreateUser_Returns_422()
        {
            UserClient userClient = new UserClient(new HttpClientTester());
        }


        [Fact]
        public void LoginUser_Returns_Token()
        {
            UserClient userClient = new UserClient(new HttpClientTester());
            userClient.UserName = "username";
            userClient.LoginUser("password");
        }

        [Fact]
        public void LoginUser_Returns_404()
        {
            UserClient userClient = new UserClient(new HttpClientTester());
        }
    }
}