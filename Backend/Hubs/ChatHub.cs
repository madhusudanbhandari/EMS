using Microsoft.AspNetCore.SignalR;

namespace Backend.Hub;

public class ChatHub : global::Microsoft.AspNetCore.SignalR.Hub
{
    // public async Task SendMessage(string message)
    // {
    //     await Clients.All.SendAsync("ReceiveMessage",message);
    // }

    public async Task GetMyUserId()
    {
        var userId=Context.UserIdentifier;

        await Clients.Caller.SendAsync(
            "ReceiveUserId",
            userId
        );
    }
}