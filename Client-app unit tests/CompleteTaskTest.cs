using System;
using System.Diagnostics;
using System.IO;
using System.Security;
using IronPython.Hosting;
using Xunit;
using Xunit.Abstractions;

namespace Client_app
{
    public class CompleteTaskTest
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private string _pathToPython = "/usr/bin/python3";
        private string _fileToRun = "/home/test.py";

        public CompleteTaskTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void BaseTest()
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = _pathToPython;
            start.Arguments = $"\"{_fileToRun}\"";
            start.UseShellExecute = false;
            start.CreateNoWindow = true;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;

            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string standardError = process.StandardError.ReadToEnd();
                    string result = reader.ReadToEnd();
                    _testOutputHelper.WriteLine(result);
                    _testOutputHelper.WriteLine(standardError);
                }
            }
        }

        [Fact]
        public void OtherBaseTest()
        {
            var engine = Python.CreateEngine();


            // var a = AppDomain.CreateDomain("/home/ane/Documents/GitHub/P7-Client/Client-app unit tests/");
            var a = AppDomain.CurrentDomain.DynamicDirectory;
            var b = AppDomain.CurrentDomain.BaseDirectory;

            var c = 0;
            
            var source =
                engine.CreateScriptSourceFromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "/../../../test.py"));

            var scope = engine.CreateScope();

            var n = source.Execute(scope);

            _testOutputHelper.WriteLine(n);
        }
    }
}