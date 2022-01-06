using System;
using System.Collections.Generic;
using System.IO;

namespace Client.Models
{
    public class CompletedTask
    {
        public string Id { get; }
        
        public string Number { get; }
        
        public string SubNumber { get; }

        public Stream FileStream { get; }

        public CompletedTask(string id, string number, string subNumber, Stream fileStream)
        {
            Id = id;
            Number = number;
            SubNumber = subNumber;
            FileStream = fileStream;
        }
    }
}
