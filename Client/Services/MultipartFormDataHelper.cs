using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace Client.Services
{
    public class MultipartFormDataHelper
    {
        public static MultipartFormDataContent CreateContent(IDictionary<string, string> formdata, IDictionary<string, Stream> files)
        {
            MultipartFormDataContent content = new MultipartFormDataContent();

            foreach (KeyValuePair<string, string> formEntry in formdata)
                content.Add(new StringContent(formEntry.Value), formEntry.Key);

            foreach (KeyValuePair<string, Stream> file in files)
            {
                file.Value.Position = 0;
                long streamLength = file.Value.Length;
                byte[] buffer = new byte[streamLength];
                file.Value.Read(buffer, 0, (int)streamLength);

                content.Add(new ByteArrayContent(buffer, 0, buffer.Length), file.Key, file.Key);
            }
            return content;
        }
    }
}
