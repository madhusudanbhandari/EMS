namespace Backend.Dtos.Chat;

public class MessageResponseDto
{
    public int Id{get;set;}
    public int ConversationId{get;set;}
    public int SenderId{get;set;}

    public string Content{get;set;}=string.Empty;

    public DateTime SentAt{get;set;}
    public bool IsRead{get;set;}
}