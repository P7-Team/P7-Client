namespace Client.Interfaces
{
    public interface IHeartbeatClient
    {
        public bool SendHeartbeatWorking();

        public bool SendHeartbeatDone();

        public bool SendHeartbeatShuttingDown();
    }
}