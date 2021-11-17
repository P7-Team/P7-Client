using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text.Json.Serialization;
using Client.Interfaces;
using Client.Models;
using Newtonsoft.Json;

namespace Client.Clients
{
     
    public class HeartbeatClient
    {
        //TODO: Make methods so that the client is able to send heartbeats, telling the scheduler of our status. Send the info as JSON file.
        // Currently types of heartbeats are Working, Done, ShuttingDown.
        // Working: I'm working on a task. Done: I'm done working on a task, please acknowledge. Shuttingdown: I'm being shut down, reschedule my task.
        
        private IHttpClient _client;
        public HeartbeatClient(IHttpClient client)
        {
            _client = client;
        }
        
    }
}
