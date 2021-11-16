using Client.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Client_app
{
    public class MultipartFormDataHelperTests
    {
        [Fact]
        public void CreateContent_CreatesRequestWithMultipartFormDataContentType()
        {
            MultipartFormDataContent content = MultipartFormDataHelper.CreateContent(new Dictionary<string, string>(), new Dictionary<string, Stream>());

            Assert.Equal("multipart/form-data", content.Headers.ContentType.MediaType);
        }

        [Fact]
        public async Task CreateContent_GivenFormData_AddsFormDataToContent()
        {
            MultipartFormDataContent content = MultipartFormDataHelper.CreateContent(new Dictionary<string, string> {
                { "id", "testValue" }
            }, new Dictionary<string, Stream>());

            string contentString = await content.ReadAsStringAsync();

            Assert.Contains("Content-Disposition: form-data; name=id", contentString);
            Assert.Contains("testValue", contentString);
        }

        [Fact]
        public void CreateContent_GivenStreams_AddsMultipartDataToContent()
        {
            MemoryStream stream = new MemoryStream();
            stream.Write(Encoding.UTF8.GetBytes("testValue"));
            MultipartFormDataContent content = MultipartFormDataHelper.CreateContent(new Dictionary<string, string>(), new Dictionary<string, Stream>() {
                {"result", stream }
            });

            string contentString = content.ReadAsStringAsync().Result;
            Assert.Contains("Content-Disposition: form-data; name=result; filename=result", contentString);
            Assert.Contains("testValue", contentString);
        }

    }
}
