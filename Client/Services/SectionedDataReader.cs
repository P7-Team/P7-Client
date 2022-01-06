
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;


namespace Client.Services
{
    public class SectionedDataReader
    {
        private readonly MultipartReader _reader;

        public SectionedDataReader(MultipartReader reader)
        {
            _reader = reader;
        }

        public async Task<MultipartSection> ReadSection()
        {
            return await _reader.ReadNextSectionAsync();
        }

        public async Task<FileStream> ReadFileSectionAsync(MultipartSection section)
        {
            var fileSection = section.AsFileSection();
            var fileName = fileSection.FileName;

            using (var stream = new FileStream(fileName, FileMode.Append))
            {
                await fileSection.FileStream.CopyToAsync(stream);
                // Not certain when stream will be disposed when returned within using block
                return stream;
            }
        }

        public async Task<KeyValuePair<string, string>> ReadFormDataSectionAsync(MultipartSection section)
        {
            var formData = section.AsFormDataSection();
            return new KeyValuePair<string, string>(formData.Name, await formData.GetValueAsync());
        }

        public string GetContentDisposition(MultipartSection section)
        {
            return section.ContentDisposition;
        }
    }
}
