using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using Client.Exceptions;

namespace Client
{
    public class PythonTaskCompleter
    {
        private readonly string _pathToPython;
        private readonly string _sourceFileName;
        private readonly string _workingDirectory;
        private string _result;

        public PythonTaskCompleter(string pathToPython, string workingDirectory, string sourceFileName)
        {
            _pathToPython = pathToPython;
            _workingDirectory = workingDirectory;
            _sourceFileName = sourceFileName;
        }

        public void Run()
        {
            var start = new ProcessStartInfo
            {
                FileName = _pathToPython,
                Arguments = $"\"{_sourceFileName}\"",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WorkingDirectory = _workingDirectory
            };

            try
            {
                using var process = Process.Start(start);
                if (process == null) return;
                using StreamReader reader = process.StandardOutput;

                var standardError = process.StandardError.ReadToEnd();

                if (string.IsNullOrEmpty(standardError))
                {
                    _result = reader.ReadToEnd();
                    _result = _result.TrimEnd('\r', '\n');
                }
                else
                {
                    throw new ArgumentException(
                        "Could not find source file, or source file is not readable. \nPython says: " + standardError);
                }
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new CompletionException("Task could not be run, path to Python is wrong. More info: " + e.Message);
            }
        }

        public string GetResult()
        {
            return _result;
        }
    }
}