using Microsoft.AspNetCore.SignalR;

namespace SignalRServer.Hubs;

public class LearningHub : Hub<ILearningHubClient>
{
    public Task BroadCastMessage(string msg)
    {
        return Clients.All.ReceiveMessage(msg);
    }

    public Task SendToOthers(string msg)
    {
        return Clients.Others.ReceiveMessage(msg);
    }

    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        return base.OnDisconnectedAsync(exception);
    }
}