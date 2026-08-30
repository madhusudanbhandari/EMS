
using Backend.Models;

namespace Backend.Interface;

public interface IChatRepository
{
    Task<Conversation?> GetConversationByIdAsync(int conversationId);
    Task<List<Conversation>> GetUserConversationsAsync(int userId);
    Task<bool> IsUserParticipantAsync(
        int conversationId,
        int userId
    );

    Task<List<Message>> GetConversationMessagesAsync(
        int conversationId
    );

    Task AddConversationAsync(
        Conversation conversation
    );

    Task AddMessageAsync(

        Message message
    );
    
    Task SaveChangesAsync();
}