namespace Client.Models
{
    public class Task
    {
        // The name of the source file
        private string _source;

        public string Id { get;  }
        public string Number { get; }
        public string SubNumber { get; }

        // private string _input;

        public Task(string source, string id, string number, string subNumber)
        {
            _source = source;
            Id = id;
            Number = number;
            SubNumber = subNumber;
        }

        public string getSource()
        {
            return _source;
        }
    }
}