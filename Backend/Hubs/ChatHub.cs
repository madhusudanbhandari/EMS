using System.Security.Claims;
using Backend.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Backend.Dtos.Chat;

namespace Backend.Hub;

[Authorize]
public class ChatHub : global::Microsoft.AspNetCore.SignalR.Hub
{
    private readonly IChatService _chatService;

    public ChatHub(IChatService chatService)
    {
        _chatService=chatService;
    }

    private int currentUserId
    {
        get
        {
            return int.Parse(
                Context.User!.FindFirstValue(
                ClaimTypes.NameIdentifier
            )!
            );
        }
    }

    public async Task JoinConversation(int conversationId)
    {
        await _chatService.JoinConversationAsync(
            currentUserId,
            conversationId
        );
     

        await Groups.AddToGroupAsync(
            Context.ConnectionId,
            GetConversationGroupName(conversationId)
        );
    }

    public async Task LeaveConversation(int conversationId)
    {
        await Groups.RemoveFromGroupAsync(
            Context.ConnectionId,
            GetConversationGroupName(conversationId)
        );
    }

    public async Task SendMessage(
        int conversationId,
        SendMessageDto dto
    )
    {
        var message=await _chatService.SendMessageAsync(
            currentUserId,
            conversationId,
            dto
        );

        await Clients
        .Group(GetConversationGroupName(conversationId))
        .SendAsync(
            "ReceiveMessage",
            message
        );
    }
    
    private static string GetConversationGroupName(int conversationId)
    {
        return $"conversation_{conversationId}";
    }
}