using System.Net;
using System.Net.Http;
using Client.Interfaces;
using System.Text;


namespace Client.Clients
{
     
    public class HeartbeatClient : IHeartbeatController
    {


        private IHttpService _client;
        public HeartbeatClient(IHttpService client)
        {
            _client = client;
        }

        private bool SendHeartbeat(string messageType)
        {
         
            HttpContent content = new StringContent(
                "{\"MessageType\":\"#type#\"}".Replace("#type#", messageType),
                Encoding.UTF8,
                "application/json"); //Type of Content

            HttpResponseMessage response = _client.Post("api/HeartBeat", content);

            HttpStatusCode statusOK = HttpStatusCode.OK; 

            return response.StatusCode.Equals(statusOK);
            
        }

        public bool SendHeartbeatWorking()
        {
            return SendHeartbeat("Working");
        }

        public bool SendHeartbeatDone()
        {
            return SendHeartbeat("Done");
        }

        public bool SendHeartbeatShuttingDown()
        {
            return SendHeartbeat("ShuttingDown");
        }
    }
}
