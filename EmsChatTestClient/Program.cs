using Microsoft.AspNetCore.SignalR.Client;

Console.WriteLine("Starting SignalR test client...");

Console.Write("Enter JWT token: ");

var token = Console.ReadLine();

var connection = new HubConnectionBuilder()
    .WithUrl(
        "http://localhost:5062/ChatHub",
        options =>
        {
            options.AccessTokenProvider = () =>
                Task.FromResult(token)!;
        })
    .Build();

Console.WriteLine("Connecting...");

connection.On<object>(
    "ReceiveMessage",
    message =>
    {
        Console.WriteLine();
        Console.WriteLine(" Message Received");
        Console.WriteLine(message);
        Console.Write(">");
    }
);

await connection.StartAsync();

Console.WriteLine("✅ Connected!");

Console.WriteLine("Enter your conversation ID:");

var conversationId=
                int.Parse(Console.ReadLine()!);
        
await connection.InvokeAsync("JoinConversation",conversationId);



Console.WriteLine($"Joined conversation {conversationId}");

Console.WriteLine();
Console.WriteLine("Type a message.");
Console.WriteLine("Type 'exit' to quit");

while (true)
{
    Console.WriteLine(">");

    var message=Console.ReadLine();

    if(message?.ToLower()=="exit")
    break;

    if(string.IsNullOrWhiteSpace(message))
    continue;

    await connection.InvokeAsync(
        "SendMessage",
        conversationId,
        new
        {
            Content=message
        }
    );
}