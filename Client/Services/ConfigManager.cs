using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Client.Exceptions;
using Client.Interfaces;
using Newtonsoft.Json;

namespace Client.Services
{
    public class ConfigManager : IConfigManager
    {
        private readonly string _configPath;
        private readonly string _interpreterPath;
        private readonly string _workingDirectory;
        private Dictionary<string, string> _config;

        public ConfigManager()
        {
            _configPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "config.json";
            _interpreterPath = "InterpreterPath";
            _workingDirectory = "WorkingDirectory";
            _config = new Dictionary<string, string>();
        }

        /// <summary>
        /// Returns the key-value paris of the config file as a dictionary.
        /// If no config exists, this will be added.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetConfig()
        {
            OpenOrCreateConfig();

            return _config;
        }

        /// <summary>
        /// Opens or creates the config file.
        /// </summary>
        /// <exception cref="ConfigException">Throws an exception if the file exists, but do not contain the required fields.</exception>
        private void OpenOrCreateConfig()
        {
            if (File.Exists(_configPath))
            {
                FileStream file = File.Open(_configPath, FileMode.OpenOrCreate);
                string fileInput = file.ToString();
                file.Close();
                if (fileInput == null)
                {
                    WriteConfig();
                }

                _config = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(_configPath));
                if (_config.ContainsKey(_interpreterPath) && _config.ContainsKey(_workingDirectory) &&
                    (_config[_interpreterPath] == "" || _config[_workingDirectory] == ""))
                {
                    throw new ConfigException("One or more fields of the config have not been populated");
                }
            }
            else
            {
                WriteConfig();
            }
        }

        /// <summary>
        /// Writes the required fields to the dictionary.
        /// </summary>
        private void WriteConfig()
        {
            Dictionary<string, string> template = new Dictionary<string, string>();
            template["InterpreterPath"] = "";
            template["WorkingDirectory"] = "";
            FileStream fileStream = File.Create(_configPath);
            fileStream.Close();
            File.WriteAllText(_configPath, JsonConvert.SerializeObject(template));
        }
    }
}