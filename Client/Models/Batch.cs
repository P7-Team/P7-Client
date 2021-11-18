using System.Collections.Generic;
using System.IO;

namespace Client.Models
{
    public class Batch
    {
        public string Id { get; set; }
        public Stream Executable { get; set; }
        public List<Stream> Inputs { get; set; }

        public Batch(string id, Stream executable, List<Stream> inputs)
        {
            Id = id;
            Executable = executable;
            Inputs = inputs;
        }
    }
}