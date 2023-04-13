using Microsoft.AspNetCore.SignalR.Client;

var url = "http://localhost:5069/learning-hub";

var conn = new HubConnectionBuilder()
                            .WithUrl(url)
                            .Build();
                            
conn.On<string>("ReceiveMessage", msg =>
{
    Console.WriteLine(msg);
});

try
{
    await conn.StartAsync();
    Console.WriteLine("Connected to SignalR Server");

    while (true)
    {
        var msg = string.Empty;

        Console.WriteLine("Please specify the action:");
        Console.WriteLine("0 - broadcast to all");
        Console.WriteLine("exit - Exit the program");
        
        var action = Console.ReadLine();
        if (action == "exit")
            break;
        
        Console.WriteLine("Please specify the message:");
        msg = Console.ReadLine();
        
        await conn.SendAsync("BroadcastMessage", msg);
    }
}catch(Exception ex)
{
    Console.WriteLine(ex.Message);
    Console.ReadKey();
}