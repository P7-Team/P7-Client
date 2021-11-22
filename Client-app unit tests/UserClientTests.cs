using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Client.Clients;
using Client.Interfaces;
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
            string output = content.ReadAsStringAsync().Result;
            // Parsed input
            Dictionary<string, string> dictionary =
                JsonConvert.DeserializeObject<Dictionary<string, string>>(output);

            // Already existing users in the system
            List<string> users = new List<string> {"username", "GenericUsername", "TotallyNotHacker"};

            // Username and password, for the user "username"
            string username = "username";
            string hashedPassword = "$2a$11$i7v7o31a25APe3LUJ353F.OO/qy7ujlq.9SK3osK1.WSJReOTa.JS";


            HttpResponseMessage responseMessage = new HttpResponseMessage();

            // Endpoints
            if (uri.EndsWith("login"))
            {
                if (username != dictionary["username"] ||
                    !BCrypt.Net.BCrypt.Verify(dictionary["password"], hashedPassword))
                    return new HttpResponseMessage(HttpStatusCode.NotFound);

                responseMessage.Content =
                    new StringContent(dictionary["username"] + dictionary["password"]);
                responseMessage.StatusCode = HttpStatusCode.Accepted;
                return responseMessage;
            }

            if (uri.EndsWith("signup"))
            {
                if (users.Contains(dictionary["username"]))
                {
                    responseMessage.StatusCode = HttpStatusCode.UnprocessableEntity;
                    return responseMessage;
                }

                responseMessage.StatusCode = HttpStatusCode.Accepted;
                return responseMessage;
            }

            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        public HttpResponseMessage Delete(string uri)
        {
            throw new System.NotImplementedException();
        }
    }

    public class UserClientTests
    {
        [Fact]
        public void LoginUser_Can_Login_Returns_Token()
        {
            UserClient userClient = new UserClient(new HttpClientTester());
            string username = "username";
            string password = "password";
            string expectedToken = username + password;
            // As the token in the test implementation is equals to username+password, this check will suffice.
            
            string actualToken = userClient.LoginUser(username, password);
            
            Assert.Equal(expectedToken, actualToken);
        }

        [Fact]
        public void LoginUser_Cannot_Login_Returns_NoToken()
        {
            UserClient userClient = new UserClient(new HttpClientTester());
            Assert.Empty(userClient.LoginUser("username", "SomeWrongPassword"));
        }

        [Fact]
        public void CreateUser_Can_Create_User_Returns_True()
        {
            UserClient userClient = new UserClient(new HttpClientTester());
            Assert.True(userClient.CreateUser("UniqueUsername", "SomeWrongPassword"));
        }

        [Fact]
        public void CreateUser_Cannot_Create_User_Returns_False()
        {
            UserClient userClient = new UserClient(new HttpClientTester());
            Assert.False(userClient.CreateUser("GenericUsername", "SomeWrongPassword"));
        }
    }
}