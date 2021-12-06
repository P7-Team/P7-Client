using System;

namespace Client.Exceptions
{
    public class CompletionException : Exception
    {
        public CompletionException(string message) : base(message)
        {
        // Intentionally left empty...
        }
    }
}