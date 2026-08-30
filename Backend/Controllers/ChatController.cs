

using System.Security.Claims;
using Backend.Dtos.Chat;
using Backend.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/chat")]
[Authorize]

public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService=chatService;
    }

    private int currentUserId
    {
        get
        {
            var claim=User.FindFirstValue(ClaimTypes.NameIdentifier);

            return int.Parse(claim!);

        }
    }

    [HttpPost("conversations")]
    public async Task<IActionResult> CreateConversation(CreateConversationDto dto)
    {
        var result=await _chatService.CreateConversationAsync(
            currentUserId,
            dto
        );

        return Ok(result
        );
    }

    [HttpGet("conversations")]
    public async Task<IActionResult> GetMyConversation()
    {
        var result=await _chatService.GetMyConversationsAsync(currentUserId);

        return Ok(result);
    }

    [HttpGet("conversations/{conversationId}/messages")]
    public async Task<IActionResult> GetMessages(int conversationId)
    {
        try
        {
            var result=await _chatService.GetMessagesAsync(currentUserId,conversationId);
            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [HttpPost("conversations/{conversationId}/participants")]
    public async Task<IActionResult> AddParticipant(
        int conversationId,
        AddParticipantDto dto
    )
    {
        try
        {
            await _chatService.AddParticipantAsync(
                conversationId,
                dto.UserId
            );

            return Ok(
                new
                {
                    message="Participant added successfully"
                }
            );
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [HttpPost("conversations/{conversationId}/messages")]

    public async Task<IActionResult> SendMessage(
        int conversationId,
        SendMessageDto dto
    )
    {
        try
        {
            var result=await _chatService.SendMessageAsync(
                currentUserId,
                conversationId,
                dto
            );

            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch(ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}