namespace Client.Models
{
    public class Task
    {
        private string _source;

        private string _input;

        public Task(string source, string input)
        {
            _source = source;
            _input = input;
        }

        public string getSource()
        {
            return _source;
        }
    }
}