
namespace Backend.Dtos.Chat;

public class ConversationResponseDto
{
    public int Id{get;set;}

    public DateTime CreatedAt{get;set;}

    public List<int> ParticipantIds{get;set;}=new();
}