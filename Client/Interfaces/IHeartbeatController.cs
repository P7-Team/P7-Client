namespace Client.Interfaces
{
    public interface IHeartbeatController
    {
        public bool SendHeartbeatWorking();

        public bool SendHeartbeatDone();

        public bool SendHeartbeatShuttingDown();
    }
}