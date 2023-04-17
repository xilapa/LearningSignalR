using MessagePack;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;

var url = "http://localhost:5069/learning-hub";

var conn = new HubConnectionBuilder()
    .WithUrl(url, 
        HttpTransportType.WebSockets | HttpTransportType.ServerSentEvents | HttpTransportType.LongPolling,
        opts =>
    {
        opts.DefaultTransferFormat = TransferFormat.Binary;
    })
    .AddMessagePackProtocol(opt =>
    {
        opt.SerializerOptions = MessagePackSerializerOptions.Standard
            .WithOmitAssemblyVersion(true);
    })
    .Build();

conn.On<string>("ReceiveMessage", msg => Console.WriteLine(msg));
conn.On<string>("PrivateMessage", msg => Console.WriteLine(msg));

try
{
    await conn.StartAsync();
    Console.WriteLine("Connected to SignalR Server");

    while (true)
    {
        var msg = string.Empty;

        Console.WriteLine("Please specify the action:");
        Console.WriteLine("0 - broadcast to all");
        Console.WriteLine("1 - send to others");
        Console.WriteLine("2 - send to caller");
        Console.WriteLine("3 - get connection id");
        Console.WriteLine("4 - send a private message to a specific user");
        Console.WriteLine("5 - send message to a group");
        Console.WriteLine("6 - join a group");
        Console.WriteLine("7 - leave a group");
        Console.WriteLine("exit - Exit the program");

        var action = Console.ReadLine();
        if (action == "exit")
            break;
        
        var groupName = string.Empty;
        if (action is "5" or "6" or "7")
        { 
            Console.WriteLine("Please specify the group name:");
            groupName = Console.ReadLine();
        }


        Console.WriteLine("Please specify the message:");
        msg = Console.ReadLine();

        switch (action)
        {
            case "0":
                await conn.SendAsync("BroadcastMessage", msg);
                break;
            case "1":
                await conn.SendAsync("SendToOthers", msg);
                break;
            case "2":
                await conn.SendAsync("SendToCaller", msg);
                break;
            case "3":
                await conn.SendAsync("GetConnectionId");
                break;
            case "4":
                Console.WriteLine("Please specify the user id:");
                var id = Console.ReadLine();
                await conn.SendAsync("SendMessageTo", msg, id);
                break;
            case "5":
                await conn.SendAsync("SendToGroup", msg, groupName);
                break;
            case "6":
                await conn.SendAsync("JoinGroup", groupName);
                break;
            case "7":
                await conn.SendAsync("LeaveGroup", groupName);
                break;
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    Console.ReadKey();
}