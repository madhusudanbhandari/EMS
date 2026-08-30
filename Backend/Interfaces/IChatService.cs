using Backend.Dtos.Chat;

namespace Backend.Interface;

public interface IChatService
{
    Task<ConversationResponseDto> CreateConversationAsync(
        int currentUserId,
        CreateConversationDto dto
    );

    Task<List<ConversationResponseDto>> GetMyConversationsAsync(
        int currentUserId
    );

    Task<List<MessageResponseDto>> GetMessagesAsync(
        int currentUsrId,
        int conversationId
    );

    Task JoinConversationAsync(int curretntUserId, int conversationId);

    Task<MessageResponseDto> SendMessageAsync(
    int currentUserId, 
    int conversationId,
     SendMessageDto dto);

}