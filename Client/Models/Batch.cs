using System.Collections.Generic;
using System.IO;

namespace Client.Models
{
    public class Batch
    {
        public string Id { get; set; }
        public UserFile Source { get; set; }
        public List<UserFile> Inputs { get; set; }
        public string Language { get; set; }

        public Batch(string id, UserFile source, string language, List<UserFile> inputs)
        {
            Id = id;
            Source = source;
            Inputs = inputs;
            Language = language;
        }
    }
}