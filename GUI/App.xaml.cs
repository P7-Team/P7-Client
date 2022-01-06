using System;
using System.Windows;
using Client.Services;
using Microsoft.Extensions.Configuration;

namespace GUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            IConfigurationRoot config = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json").Build();
            string ip = config.GetValue<string>("IP");
            HttpService.GetHttpService( ip);
        }
    }
}
