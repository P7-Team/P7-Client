namespace Client.Interfaces
{
    public interface IUserClient
    {
        public string Token { get; }

        public bool Login(string username, string password);

        public bool CreateUser(string username, string password);
    }
}