using System.IO;
using System.Text;

namespace Client.Models
{
    public class UserFile
    {
        public string Name { get; set; }
        public Stream Data { get; set; }
        public Encoding Enc { get; set; }

        public UserFile(string filename, Stream fileData) : this(filename, fileData, Encoding.UTF8)
        {
            // Intentionally left empty...
        }

        public UserFile(string filename, Stream fileData, Encoding encoding)
        {
            Name = filename;
            Data = fileData;
            Enc = encoding;
        }
    }
}