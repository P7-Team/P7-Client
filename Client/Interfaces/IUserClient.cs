namespace Client.Interfaces
{
    public interface IUserClient
    {
        public string LoginUser(string username, string password);

        public bool CreateUser(string username, string password);
    }
}