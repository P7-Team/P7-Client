using System.Net;
using System.Net.Http;
using Client.Interfaces;
using System.Text;


namespace Client.Clients
{
     
    public class HeartbeatClient
    {
        //TODO: Make methods so that the client is able to send heartbeats, telling the scheduler of our status. Send the info as JSON file.
        // Currently types of heartbeats are Working, Done, ShuttingDown.
        // Working: I'm working on a task. Done: I'm done working on a task, please acknowledge. Shuttingdown: I'm being shut down, reschedule my task.


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

            HttpResponseMessage respone = _client.Post("api/HeartBeat", content);

            HttpStatusCode statusOK = HttpStatusCode.OK; 

            if (respone.StatusCode.Equals(statusOK))
            {
                return true;
            }
            else
            {
                return false;
            }
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
