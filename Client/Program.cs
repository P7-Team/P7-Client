using System;
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
            IHttpService httpService = new HttpService("http://127.0.0.1:5000/","True");
            BatchClient bob = new BatchClient(httpService);
            //bob.GetBatchStatus();
             bob.GetResult();
            
            Console.Read(); 
        }
    }
}