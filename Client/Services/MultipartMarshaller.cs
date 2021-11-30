using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Services

{
    public class MultipartMarshaller<T>
    {
        private readonly ISectionedDataReader<T> _reader;
        private List<FileStream> _fileStreams;
        private IDictionary<string, string> _formData;
        private SectionedDataReader reader;

        public MultipartMarshaller(ISectionedDataReader<T> reader)
        {
            _reader = reader;
            _fileStreams = new List<FileStream>();
            _formData = new Dictionary<string, string>();
            readContent();
        }

        public MultipartMarshaller(SectionedDataReader reader)
        {
            this.reader = reader;
        }

        public Dictionary<string, string> GetFormData()
        {
            return (Dictionary<string, string>)_formData;
        }

        public List<FileStream> GetFileStreams()
        {
            return _fileStreams;
        }

        private async void readContent()
        {
            var section = _reader.ReadSection();

            while (await section != null)
            {
                await readSectionAsync(await section);
                section = _reader.ReadSection();
            }
        }

        private async Task readSectionAsync(T section)
        {
            if (MultipartHelper.IsFileData(_reader.GetContentDisposition(section)))
                _fileStreams.Add(await _reader.ReadFileSectionAsync(section));
            else
                _formData.Add(await _reader.ReadFormDataSectionAsync(section));
        }
    }
}
