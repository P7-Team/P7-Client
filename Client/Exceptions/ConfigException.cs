using System;

namespace Client.Exceptions
{
    public class ConfigException : Exception
    {
        public ConfigException(string message) : base(message)
        {
            // Intentionally left empty...
        }
    }
}