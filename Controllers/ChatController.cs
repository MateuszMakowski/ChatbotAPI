using ChatbotAPI.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public class ChatController : ControllerBase
{
    private readonly ChatService _chatService;

    public ChatController(ChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpGet("sessions")]
    public async Task<IActionResult> GetChatSessions()
    {
        var sessions = await _chatService.GetChatSessionsAsync();
        return Ok(sessions);
    }

    [HttpPost("sessions")]
    public async Task<IActionResult> CreateChatSession([FromBody] string name)
    {
        if (string.IsNullOrEmpty(name))
            return BadRequest("Name cannot be empty.");

        var session = await _chatService.CreateChatSessionAsync(name);
        return Ok(session);
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetChatHistory([FromQuery] int sessionId)
    {
        var messages = await _chatService.GetChatHistoryAsync(sessionId);
        return Ok(messages);
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendMessage(int sessionId, [FromBody] string userMessage)
    {
        if (string.IsNullOrEmpty(userMessage))
            return BadRequest("Message cannot be empty.");

        var message = await _chatService.SendMessageAsync(sessionId, userMessage);
        return Ok(message);
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateMessage(int id, [FromBody] string botResponse)
    {
        if (string.IsNullOrEmpty(botResponse))
            return BadRequest("Response cannot be empty.");

        var success = await _chatService.UpdateMessageAsync(id, botResponse);
        return success ? NoContent() : NotFound();
    }

    [HttpPut("rate/{id}")]
    public async Task<IActionResult> RateMessage(int id, [FromBody] int rating)
    {
        var success = await _chatService.RateMessageAsync(id, rating);
        return success ? NoContent() : NotFound();
    }
}