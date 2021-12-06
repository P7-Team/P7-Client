
﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using Client.Clients;
using Client.Interfaces;
using Client.Services;

namespace Client
{
    class Program
    {
        static readonly System.Net.Http.HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {

            IHttpService httpService = new HttpService("http://164.90.236.116:80/");
            // TODO: Handle that the client crashes if there is no connection to the service
            // IUserClient userClient = new UserClient(httpService);
            // httpService.SetToken(userClient.LoginUser("username","password"));
            
            ClientStateManager clientStateManager = new ClientStateManager(httpService);
            clientStateManager.Run();
        }
    }
}