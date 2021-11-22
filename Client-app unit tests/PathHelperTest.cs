using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Xunit;

namespace Client_app
{
    public class PathHelperTest
    {
        private char sep = Path.DirectorySeparatorChar;

        private string[] randomDirNames = new string[]
        {
            "Foo", "Bar", "Bafoon", "Hacker", "Security", "Secret", "templa", "electricBoogaloo", "TailSpin", "Pumbaa"
        };

        private readonly Random _randy = new Random();

        [Fact]
        public void ExtractPath_SeparatorAtEnd_WithExt_ReturnEmpty()
        {
            // Setup
            string path = CreateWeirdPath();
            
            // Act
            string actual = PathHelper.ExtractFileNameFromPath(path);
            
            // Assert
            Assert.Empty(actual);
        }

        [Fact]
        public void ExtractPath_PathEndingInFileWithExt_DefaultExtHandling_ReturnFilenameWithExt()
        {
            // Setup
            string path = CreateWeirdPath();
            const string file = "largeProgram.hs";
            path += file;
            
            // Act
            string actual = PathHelper.ExtractFileNameFromPath(path);
            
            // Assert
            Assert.Equal(file, actual);
            
        }

        [Fact]
        public void ExtractPath_PathEndingInFileWithExt_ExplicitExtHandling_ReturnFilenameWithoutExt()
        {
            // Setup
            string path = CreateWeirdPath();
            const string file = "prologProgram";
            const string extension = ".pl";
            const bool removeExtension = true;
            path += file + extension;
            
            // Act
            string actual = PathHelper.ExtractFileNameFromPath(path, removeExtension);
            
            // Assert
            Assert.Equal(file, actual);
        }

        [Fact]
        public void ExtractPath_PathEndingWithSeparator_ExplicitExtHandling_ReturnEmpty()
        {
            // Setup
            string path = CreateWeirdPath();
            const bool removeExtension = true;

            // Act
            string actual = PathHelper.ExtractFileNameFromPath(path, removeExtension);
            
            // Assert
            Assert.Empty(actual);
        }

        [Fact]
        public void ExtractPath_PathContainsSeparatorInPath_FileWithoutExt_RemoveExt_ReturnsFile()
        {
            // Setup
            string path = CreateWeirdPath();
            path += ".ssh" + sep + RandomDirName() + sep;
            const bool removeExtension = true;
            const string file = "noExt";
            path += file;

            // Act
            string actual = PathHelper.ExtractFileNameFromPath(path, removeExtension);
            
            // Assert
            Assert.Equal(file, actual);
        }
        
        [Fact]
        public void ExtractPath_PathContainsSeparatorInPath_FileWithExt_RemoveExt_ReturnsFileWithoutExt()
        {
            // Setup
            string path = CreateWeirdPath();
            path += ".ssh" + sep + RandomDirName() + sep;
            const bool removeExtension = true;
            const string file = "noExt";
            const string ext = ".ext";
            path += file + ext;

            // Act
            string actual = PathHelper.ExtractFileNameFromPath(path, removeExtension);
            
            // Assert
            Assert.Equal(file, actual);
        }

        # region Helper Methods
        
        private string CreateWeirdPath()
        {
            string path = "Weird ass OS" + sep;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                path = "C:" + sep;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                path = sep + "home" + sep;
            }

            int numberOfDirs = _randy.Next(0, 5);

            for (int i = 0; i < numberOfDirs; i++)
            {
                path += RandomDirName() + sep;
            }

            return path;
        }

        private string RandomDirName()
        {
            int idx = _randy.Next(0, randomDirNames.Length);
            return randomDirNames[idx];
        }
        
        # endregion
    }
}