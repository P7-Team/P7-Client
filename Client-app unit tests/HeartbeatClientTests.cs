using System;

using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using Client.Clients;
using Client.Interfaces;
using Client.Models;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Xunit;


namespace Client_app
{
    public class HeartbeatClientTests
    {
        
        public HeartbeatClientTests()
        {
            
        }

        //TODO - TEST: Recieve OK from method Working, Done, ShuttingDown
        //TODO - TEST: Received Error for sending nonsense

        [Fact]
        public void TestRecieveOK()
        {

            
            
        }

        [Fact]
        public void TestStatusInvalid()
        {
            var mockHandler = new Mock<HttpMessageHandler>();

        }

        [Fact]
        public void TestsIsRun()
        {
            Assert.Equal(2, 2);
        }
    }
}
