using Microsoft.AspNetCore.SignalR.Client;

var url = "http://localhost:5069/learning-hub";

var conn = new HubConnectionBuilder()
    .WithUrl(url)
    .Build();

conn.On<string>("ReceiveMessage", msg => { Console.WriteLine(msg); });

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
        Console.WriteLine("exit - Exit the program");

        var action = Console.ReadLine();
        if (action == "exit")
            break;


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
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    Console.ReadKey();
}