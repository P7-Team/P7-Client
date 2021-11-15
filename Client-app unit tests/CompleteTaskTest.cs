using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Security;
using Client;
using Client.Exceptions;
using IronPython.Hosting;
using Xunit;
using Xunit.Abstractions;

namespace Client_app
{
    public class CompleteTaskTest
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private string _pathToPython = "/usr/bin/python3";
        private string _pathToDirectory;
        
        public CompleteTaskTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            var currentDirectory = Directory.GetCurrentDirectory();
            var oneUp = Directory.GetParent(currentDirectory).ToString();
            var twoUp = Directory.GetParent(oneUp).ToString();
            _pathToDirectory = Directory.GetParent(twoUp) + Path.DirectorySeparatorChar.ToString() + "CompleteTaskTestInputs" + Path.DirectorySeparatorChar.ToString();
        }

        [Fact]
        public void PathToPythonIsEmpty_CompletionExceptionThrown()
        {
            var completer = new PythonTaskCompleter("", _pathToDirectory, "test1.py");
            
            try
            {
                completer.Run();
                Assert.True(false);
            }
            catch (CompletionException e)
            {
                _testOutputHelper.WriteLine(e.Message);
                Assert.True(true);
            }
        }
        
        [Fact]
        public void PathToPythonIsNotEmptyButPointsToWrongFile_CompletionExceptionThrown()
        {
            var completer = new PythonTaskCompleter("/home/index.html", _pathToDirectory, "test1.py");
            
            try
            {
                completer.Run();
                Assert.True(false);
            }
            catch (CompletionException e)
            {
                _testOutputHelper.WriteLine(e.Message);
                Assert.True(true);
            }
        }
        
        [Fact]
        public void PathToPythonIsNotEmptyButPointsToDirectory_CompletionExceptionThrown()
        {
            var completer = new PythonTaskCompleter("/home", _pathToDirectory, "test1.py");
            
            try
            {
                completer.Run();
                Assert.True(false);
            }
            catch (CompletionException e)
            {
                _testOutputHelper.WriteLine(e.Message);
                Assert.True(true);
            }
        }

        [Fact]
        public void PathToSourceIsEmpty_ArgumentExceptionThrown()
        {
            var completer = new PythonTaskCompleter(_pathToPython, "", "test1.py");

            try
            {
                completer.Run();
                Assert.True(false);
            }
            catch (ArgumentException e)
            {
                _testOutputHelper.WriteLine(e.Message);
                Assert.True(true);
            }
        }
        
        [Fact]
        public void PathToSourceIsNotEmptyButWrong_ArgumentExceptionThrown()
        {
            var completer = new PythonTaskCompleter(_pathToPython, "/", "test1.py");

            try
            {
                completer.Run();
                Assert.True(false);
            }
            catch (ArgumentException e)
            {
                _testOutputHelper.WriteLine(e.Message);
                Assert.True(true);
            }
        }

        [Fact]
        public void SourceDoesNotUseOtherInput_ResultIsOutput()
        {
            var completer = new PythonTaskCompleter(_pathToPython, _pathToDirectory, "test1.py");
            
            completer.Run();

            var expected = "This is a test";
            var actual = completer.GetResult();
            
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SourceUsesInputFile_ResultIsOutput()
        {
            var completer = new PythonTaskCompleter(_pathToPython, _pathToDirectory, "test2.py");
            
            completer.Run();

            var expected = 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10;
            var actual = completer.GetResult();
            
            Assert.Equal(expected.ToString(), actual);
        }

        [Fact]
        public void MultiplePrints_ResultIsComposedOfMultipleLinesWithNewlines()
        {
            var completer = new PythonTaskCompleter(_pathToPython, _pathToDirectory, "test3.py");
            
            completer.Run();

            var expected = "This is line one\nThis is line two";
            var actual = completer.GetResult();
            
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void NewlineInMiddleOfOutput_NewlineIsKept()
        {
            var completer = new PythonTaskCompleter(_pathToPython, _pathToDirectory, "test4.py");
            
            completer.Run();

            var expected = "This is line one\nThis is line two";
            var actual = completer.GetResult();
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void NewlinesAtEndOfResult_NewlinesRemoved()
        {
            var completer = new PythonTaskCompleter(_pathToPython, _pathToDirectory, "test5.py");
            
            completer.Run();

            var expected = "This has multiple newlines at the end";
            var actual = completer.GetResult();
            
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReturnsAtEndOfResult_ReturnsRemoved()
        {
            var completer = new PythonTaskCompleter(_pathToPython, _pathToDirectory, "test6.py");
            
            completer.Run();

            var expected = "This has multiple returns at the end";
            var actual = completer.GetResult();
            
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReturnsAndNewlinesAtEndOfResult_ReturnsAndNewlinesRemoved()
        {
            var completer = new PythonTaskCompleter(_pathToPython, _pathToDirectory, "test7.py");
            
            completer.Run();

            var expected = "This has multiple returns and newlines at the end";
            var actual = completer.GetResult();
            
            Assert.Equal(expected, actual);
        }
    }
}