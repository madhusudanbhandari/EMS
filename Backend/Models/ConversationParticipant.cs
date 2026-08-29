
namespace Backend.Models;

public class ConversationParticipant
{
    public int Id{get;set;}
    public int ConversationId{get;set;}
    public Conversation Conversation{get;set;}=null!;

    public int UserId{get;set;}
    public DateTime JoinedAt{get;set;}=DateTime.UtcNow;

}