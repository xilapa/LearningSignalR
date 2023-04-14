namespace SignalRServer;

public interface ILearningHubClient
{
    Task ReceiveMessage(string msg);
    Task PrivateMessage(string msg);
}