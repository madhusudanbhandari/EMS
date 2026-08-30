

using Backend.Dtos.Chat;
using Backend.Interface;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Service;

public class ChatService : IChatService
{
    private readonly IChatRepository _chatRepository;
    public ChatService(IChatRepository chatRepository)
    {
        _chatRepository=chatRepository;
    }

    public async Task<ConversationResponseDto>CreateConversationAsync(int currentUserId, CreateConversationDto dto)
    {
        var participantIds=dto.UserIds
        .Distinct()
        .ToList();

        if (!participantIds.Contains(currentUserId))
        {
            participantIds.Add(currentUserId);
        }

        var conversation=new Conversation();

        foreach(var userId in participantIds)
        {
            conversation.Participants.Add(
                new ConversationParticipant
                {
                    UserId=userId
                }
            );
        }

        await _chatRepository.AddConversationAsync(conversation);

        await _chatRepository.SaveChangesAsync();

        return new ConversationResponseDto
        {
            Id=conversation.Id,
            CreatedAt=conversation.CreatedAt,
            ParticipantIds=participantIds
        };

    }

    public async Task<List<ConversationResponseDto>> 
        GetMyConversationsAsync(int currentUserId)
    {
        var conversations=await _chatRepository.GetUserConversationsAsync(currentUserId);

        return conversations
        .Select(c=>new ConversationResponseDto
        {
            Id=c.Id,
            CreatedAt=c.CreatedAt,
            ParticipantIds=c.Participants
                .Select(p=>p.UserId)
                .ToList()
        })
        .ToList();
    }

    public async Task<List<MessageResponseDto>>
    GetMessagesAsync(int currentUseId, int conversationId)
    {
        var isParticipant=await _chatRepository.IsUserParticipantAsync(currentUseId,conversationId);

        if (!isParticipant)
        {
            throw new UnauthorizedAccessException(
                "You are not a participant in this conversation"
            );
        }

        var message=await _chatRepository
        .GetConversationMessagesAsync(conversationId);

        return message
        .Select(m=>new MessageResponseDto
        {
            Id=m.Id,
            ConversationId=m.ConversationId,
            SenderId=m.SenderId,
            Content=m.Content,
            SentAt=m.SentAt,
            IsRead=m.IsRead
        })
        .ToList();
    }

    public async Task JoinConversationAsync(
        int currentUserId,
        int conversationId
    )
    {
        var isParticipant=await _chatRepository.IsUserParticipantAsync(currentUserId,conversationId);

        if (!isParticipant)
        {
            throw new UnauthorizedAccessException("You are not participant in this conversation");
        }
    }

    public async Task<MessageResponseDto> SendMessageAsync(
        int currentUserId,
        int conversationId,
        SendMessageDto dto
    )
    {
        if (string.IsNullOrWhiteSpace(dto.Content))
        {
            throw new ArgumentException(
                "Message cannot be empty"
            );
        }

        var isParticipant=await _chatRepository.IsUserParticipantAsync(
             currentUserId,
            conversationId
           
        );

        if (!isParticipant)
        {
            throw new UnauthorizedAccessException(
                "you are not participant in this conversation"
            );
        }

        var message=new Message
        {
            ConversationId=conversationId,
            SenderId=currentUserId,
            Content=dto.Content.Trim(),
            SentAt=DateTime.UtcNow,
            IsRead=false
        };

        await _chatRepository.AddMessageAsync(message);

        await _chatRepository.SaveChangesAsync();

        return new MessageResponseDto
        {
            Id=message.Id,
            ConversationId=message.ConversationId,
            SenderId=message.SenderId,
            Content=message.Content,
            SentAt=message.SentAt,
            IsRead=message.IsRead
        };
    }
}