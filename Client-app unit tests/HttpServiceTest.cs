using System;
using System.Net.Http;
using Client.Interfaces;
using Client.Services;
using Xunit;
using Xunit.Abstractions;

namespace Client_app
{
    public class HttpServiceTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public HttpServiceTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact (Skip = "Will only work when the web service is actually running")]
        public void BaseTest()
        {
            const string ip = "http://164.90.236.116:80/";

            IHttpService service = new HttpService(ip, "1234");

            var message = new HttpRequestMessage(HttpMethod.Get, "/api/task/ready");

            var response = service.Send(message);

            _testOutputHelper.WriteLine(response.ToString());
            _testOutputHelper.WriteLine(response.Content.ReadAsStringAsync().Result);
        }
    }
}