using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text.Json.Serialization;
using Client.Interfaces;
using Client.Models;
using Newtonsoft.Json;
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
            var message = new HttpRequestMessage(HttpMethod.Post, "api/HeartBeat");
            
            message.Content = new StringContent(
                "{\"MessageType\":\"#type#\"}".Replace("#type#", messageType),
                Encoding.UTF8,
                "application/json"); //Type of Content

            HttpResponseMessage respone = _client.Send(message);

            if (respone.IsSuccessStatusCode)
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
