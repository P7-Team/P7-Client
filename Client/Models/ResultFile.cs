using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Client.Models
{
    public class ResultFile
    {
        public string Name { get; }
        public Stream Content { get; }
        public ResultFile(string filename, Stream content)
        {
            Name = filename;
            Content = content;
        }
    }
}
