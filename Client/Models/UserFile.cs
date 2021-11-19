using System.IO;
using System.Text;

namespace Client.Models
{
    public class UserFile
    {
        private const char EXTENSION_SEPERATOR = '.';
        public string Name { get; set; }
        public Stream Data { get; set; }
        public Encoding Encode { get; set; }

        public UserFile(string filename, Stream fileData) : this(filename, fileData, Encoding.UTF8)
        {
            // Intentionally left empty...
        }

        public UserFile(string filename, Stream fileData, Encoding encoding)
        {
            Name = filename;
            Data = fileData;
            Encode = encoding;
        }

        public static string ExtractFileNameFromPath(string absolutePath, bool removeFileExtension=false)
        {
            char seperator = Path.DirectorySeparatorChar;
            int filenameBeginIndex = absolutePath.LastIndexOf(seperator);
            int filenameEndIndex = absolutePath.Length;
            if (removeFileExtension)
            {
                filenameEndIndex = absolutePath.LastIndexOf(EXTENSION_SEPERATOR);
            }
            
            return absolutePath[filenameBeginIndex..filenameEndIndex];
        }
    }
}