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
        
        /// <summary>
        /// Sends a heartbeat of the type provided as messageType.
        /// </summary>
        /// <param name="messageType">The messageType which should be sent.</param>
        /// <returns>Returns true if the heartbeat was received, false if it wasn't.</returns>
        private bool SendHeartbeat(string messageType)
        {
         
            HttpContent content = new StringContent(
                "{\"status\":\"#type#\"}".Replace("#type#", messageType),
                Encoding.UTF8,
                "application/json"); //Type of Content

            HttpResponseMessage response = _client.Post("api/HeartBeat", content);

            HttpStatusCode statusOK = HttpStatusCode.OK; 

            return response.StatusCode.Equals(statusOK);
            
        }

        /// <summary>
        /// Sends a heartbeat with the status "Working".
        /// </summary>
        /// <returns>Returns true if the message was sent successfully, false if it wasn't.</returns>
        public bool SendHeartbeatWorking()
        {
            return SendHeartbeat("Working");
        }

        /// <summary>
        /// Sends a heartbeat with the status "Done".
        /// </summary>
        /// <returns>Returns true if the message was sent successfully, false if it wasn't.</returns>
        public bool SendHeartbeatDone()
        {
            return SendHeartbeat("Done");
        }

        /// <summary>
        /// Sends a heartbeat with the status "ShuttingDown".
        /// </summary>
        /// <returns>Returns true if the message was sent successfully, false if it wasn't.</returns>
        public bool SendHeartbeatShuttingDown()
        {
            return SendHeartbeat("ShuttingDown");
        }
    }
}
