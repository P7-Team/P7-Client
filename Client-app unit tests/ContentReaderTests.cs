using System.IO;
using System.Text;
using Client.Services;
using Xunit;

namespace Client_app
{
    public class ContentReaderTest
    {
        [Fact]
        public void ReadStreamContent_WithDefaultEncoding_StreamWithContentUsingDefaultEncoding_ReturnsCorrectString()
        {
            // Setup
            const string expected = "String content";
            MemoryStream stream = new MemoryStream();
            stream.Write(Encoding.UTF8.GetBytes(expected));
            stream.Position = 0;

            // Act
            string actual = ContentReader.ReadStreamContent(stream);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReadSreamContent_WithDefaultEncoding_StreamWithNoContent_ReturnsEmptyString()
        {
            // Setup
            MemoryStream stream = new MemoryStream();

            // Act
            string actual = ContentReader.ReadStreamContent(stream);

            // Assert
            Assert.Empty(actual);
        }

        [Fact]
        public void
            ReadStreamContent_WithDefaultEncoding_StreamWithContentUsingDifferentEncodingFamily_ReturnsIncorrectString()
        {
            // Setup
            const string original = "This is the original string";
            MemoryStream stream = new MemoryStream();
            stream.Write(Encoding.Unicode.GetBytes(original));
            stream.Position = 0;

            // Act
            string actual = ContentReader.ReadStreamContent(stream);

            // Assert
            Assert.NotEqual(original, actual);
        }

        [Fact]
        public void
            ReadStreamContent_WithUnicodeEncoding_StreamWithContentUsingSameEncoding_ReturnsCorrectString()
        {
            // Setup
            const string expected = "This is the original string";
            MemoryStream stream = new MemoryStream();
            stream.Write(Encoding.Unicode.GetBytes(expected));
            stream.Position = 0;

            // Act
            string actual = ContentReader.ReadStreamContent(stream, Encoding.Unicode);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
} 