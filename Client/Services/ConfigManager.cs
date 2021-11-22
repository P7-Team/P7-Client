using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Client.Interfaces;
using Newtonsoft.Json;

namespace Client.Services
{
    public class ConfigManager : IConfigManager
    {
        private static readonly string ConfigPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "config.json";
        private const string InterpreterPath = "InterpreterPath";
        private const string WorkingDirectory = "WorkingDirectory";
        private Dictionary<string, string> _config = new Dictionary<string, string>();

        public Dictionary<string, string> GetConfig()
        {
            Console.WriteLine(ConfigPath);
            OpenConfig();

            return _config;
        }

        private void OpenConfig()
        {
            if (File.Exists(ConfigPath))
            {
                FileStream file = File.Open(ConfigPath, FileMode.OpenOrCreate);
                string fileInput = file.ToString();
                file.Close();
                if (fileInput == null)
                {
                    WriteConfig();
                }

                _config = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(ConfigPath));
                if (_config.ContainsKey(InterpreterPath) && _config.ContainsKey(WorkingDirectory) &&
                    (_config[InterpreterPath] == "" || _config[WorkingDirectory] == ""))
                {
                    throw new Exception("One or more fields of the config have not been populated");
                }
            }
            else
            {
                WriteConfig();
            }
        }

        private void WriteConfig()
        {
            Dictionary<string, string> template = new Dictionary<string, string>();
            template["InterpreterPath"] = "";
            template["WorkingDirectory"] = "";
            FileStream fileStream = File.Create(ConfigPath);
            fileStream.Close();
            File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(template));
        }
    }
}