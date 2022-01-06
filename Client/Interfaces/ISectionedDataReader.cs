using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Services
{
    public interface ISectionedDataReader<T>
    {
        public Task<T> ReadSection();

        public Task<FileStream> ReadFileSectionAsync(T section);

        public Task<KeyValuePair<string, string>> ReadFormDataSectionAsync(T section);

        public string GetContentDisposition(T section);
    }
}
