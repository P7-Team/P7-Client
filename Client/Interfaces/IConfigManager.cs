using System.Collections.Generic;

namespace Client.Interfaces
{
    public interface IConfigManager
    {
        public Dictionary<string, string> GetConfig();
    }
}