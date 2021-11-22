using System;
using System.Diagnostics;
using System.IO;
using Client.Exceptions;
using Client.Interfaces;

namespace Client.Services
{
    public class InterpretedTaskCompleter : IInterpretedTaskCompleter
    {
        private readonly string _pathToInterpreter;
        private readonly string _sourceFileName;
        private readonly string _workingDirectory;
        private string _result;

        public InterpretedTaskCompleter(string pathToInterpreter, string workingDirectory, string sourceFileName)
        {
            _pathToInterpreter = pathToInterpreter;
            _workingDirectory = workingDirectory;
            _sourceFileName = sourceFileName;
        }

        public void Run()
        {
            var start = new ProcessStartInfo
            {
                FileName = _pathToInterpreter,
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
                        "Could not find source file, or source file is not readable. Interpreter says: " +
                        standardError);
                }
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new CompletionException("Task could not be run, path to interpreter is wrong. More info: " +
                                              e.Message);
            }
        }

        public string GetResult()
        {
            return _result;
        }
    }
}