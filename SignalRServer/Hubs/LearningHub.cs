using Microsoft.AspNetCore.SignalR;

namespace SignalRServer.Hubs;

public class LearningHub : Hub<ILearningHubClient>
{
    public Task BroadCastMessage(string msg)
    {
        return Clients.All.ReceiveMessage(FormatMessage(msg));
    }

    public Task SendToOthers(string msg)
    {
        return Clients.Others.ReceiveMessage(FormatMessage(msg));
    }

    public Task SendToCaller(string msg)
    {
        return Clients.Caller.ReceiveMessage(FormatMessage(msg));
    }

    public Task GetConnectionId()
    {
        return Clients.Caller.ReceiveMessage($"ConnectionId: {Context.ConnectionId}");
    }
    
    public Task SendMessageTo(string msg, string destinationId)
    {
        return Clients.Client(destinationId).PrivateMessage(FormatPrivateMessage(msg, Context.ConnectionId));
    }

    public Task SendToGroup(string msg, string groupName)
    {
        return Clients.Group(groupName).ReceiveMessage(FormatMessage(msg));
    }

    public async Task JoinGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await Clients.Caller.ReceiveMessage($"Current user joined to group: {groupName}");
        await Clients.Others.ReceiveMessage($"User {Context.ConnectionId} joined to group: {groupName}");
    }
    
    public async Task LeaveGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        await Clients.Caller.ReceiveMessage($"Current user has left the group: {groupName}");
        await Clients.Others.ReceiveMessage($"User {Context.ConnectionId} has left the group: {groupName}");
    }

    private string FormatMessage(string msg)
    {
        return $"User Id:{Context.ConnectionId} - Message: {msg}";
    }
    
    private string FormatPrivateMessage(string msg, string id)
    {
        return $"Private message from:{id} - Message: {msg}";
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