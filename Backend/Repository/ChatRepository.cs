

using Backend.Data;
using Backend.Interface;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository;

public class ChatRepository: IChatRepository
{
    private readonly AppDbContext _context;

    public ChatRepository(AppDbContext context)
    {
        _context=context;
    }

    public async Task<Conversation?> GetConversationByIdAsync(int conversationId)
    {
        return await _context.Conversations
        .Include(c=>c.Participants)
        .FirstOrDefaultAsync(c=>c.Id==conversationId);

    }

    public async Task<List<Conversation>> GetUserConversationsAsync(int userId)
    {
        return await _context.Conversations
        .Include(c=>c.Participants)
        .Where(c=>c.Participants
        .Any(p=>p.UserId==userId))
        .ToListAsync();
    }

    public async Task<bool> IsUserParticipantAsync(
        int userId,
    int conversationId
    )
{
    return await _context.ConversationParticipants
        .AnyAsync(cp =>
            cp.ConversationId == conversationId &&
            cp.UserId == userId);

   

  
}


    public async Task<List<Message>> GetConversationMessagesAsync(
        int conversationId
    )
    {
        return await _context.Messages
        .Where(m=>m.ConversationId==conversationId)
        .OrderBy(m=>m.SentAt)
        .ToListAsync();
    }

    public async Task AddConversationAsync(
        Conversation conversation
    )
    {
        await _context.Conversations.AddAsync(conversation);
    }

    public async Task AddMessageAsync(
        Message message
    )
    {
        await _context.Messages.AddAsync(message);
    }


    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}