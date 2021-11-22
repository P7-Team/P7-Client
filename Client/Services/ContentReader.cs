using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Client.Services
{
    public class ContentReader
    {
        private const int BUFFER_SIZE = 512;

        public static string ReadStreamContent(Stream contentStream)
        {
            return ReadStreamContent(contentStream, Encoding.UTF8);
        }

        public static string ReadStreamContent(Stream contentStream, Encoding encoding)
        {
            List<byte> bytes = new List<byte>();
            int bytesRead;
            do
            {
                byte[] buffer = new byte[BUFFER_SIZE];
                bytesRead = contentStream.ReadAsync(buffer, 0, buffer.Length).Result;
                bytes.AddRange(buffer);
            } while (bytesRead > 0);

            string result = encoding.GetString(bytes.ToArray());
            int endOfContent = result.IndexOf('\0');
            return result[0..endOfContent];
        }
    }
}