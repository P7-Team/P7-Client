using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Client.Exceptions;
using Client.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;


namespace Client.Services
{
    public class ConfigManager
    {
        private IConfigurationRoot _configurationRoot;

        public ConfigManager()
        {
            _configurationRoot = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json").Build();
        }

        /// <summary>
        /// Returns the key-value paris of the config file as a dictionary.
        /// If no config exists, this will be added.
        /// </summary>
        /// <returns></returns>
        public string GetConfig(string key)
        {
            return _configurationRoot.GetValue<string>(key);
        }
    }
}