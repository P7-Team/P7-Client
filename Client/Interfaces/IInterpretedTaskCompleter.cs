namespace Client.Interfaces
{
    public interface IInterpretedTaskCompleter
    {
        public void Run();

        public string GetResult();
    }
}