using System;
using System.Collections.Generic;
using System.IO;

namespace Client.Models
{
    public class CompletedTask
    {
        public long Id { get; }

        public Stream FileStream { get; }

        public CompletedTask(long id, Stream fileStream)
        {
            Id = id;
            FileStream = fileStream;
        }
    }
}
