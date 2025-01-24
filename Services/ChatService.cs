using ChatbotAPI.Data;
using ChatbotAPI.Models;
using ChatbotAPI.Utils;
using Microsoft.EntityFrameworkCore;

namespace ChatbotAPI.Services;

public class ChatService
{
    private readonly ChatbotDbContext _context;

    public ChatService(ChatbotDbContext context)
    {
        _context = context;
    }

    public async Task<List<ChatSession>> GetChatSessionsAsync()
    {
        return await _context.ChatSessions
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<ChatSession> CreateChatSessionAsync(string name)
    {
        var session = new ChatSession { Name = name };
        _context.ChatSessions.Add(session);
        await _context.SaveChangesAsync();
        return session;
    }

    public async Task<List<ChatMessage>> GetChatHistoryAsync(int sessionId)
    {
        return await _context.ChatMessages
            .Where(m => m.ChatSessionId == sessionId)
            .OrderBy(m => m.Timestamp)
            .ToListAsync();
    }

    public async Task<ChatMessage> SendMessageAsync(int sessionId, string userMessage)
    {
        var session = await _context.ChatSessions.FindAsync(sessionId) ??
                      new ChatSession { Name = $"Chat {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}" };

        if (session.Id == 0)
        {
            _context.ChatSessions.Add(session);
            await _context.SaveChangesAsync();
        }

        var botResponse = GenerateResponse(userMessage);

        var message = new ChatMessage
        {
            ChatSessionId = session.Id,
            UserMessage = userMessage,
            BotResponse = botResponse,
            Timestamp = DateTime.UtcNow
        };

        _context.ChatMessages.Add(message);
        await _context.SaveChangesAsync();

        return message;
    }

    public async Task<bool> UpdateMessageAsync(int messageId, string botResponse)
    {
        var message = await _context.ChatMessages.FindAsync(messageId);
        if (message == null) return false;

        message.BotResponse = botResponse;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RateMessageAsync(int messageId, int rating)
    {
        var message = await _context.ChatMessages.FindAsync(messageId);
        if (message == null) return false;

        message.Rating = rating;
        await _context.SaveChangesAsync();

        return true;
    }

    public string GenerateResponse(string userMessage)
    {
        var random = new Random();
        int category = random.Next(0, 3); // 0: Short, 1: Medium, 2: Long

        return category switch
        {
            0 => ResponseTexts.ShortResponses[random.Next(ResponseTexts.ShortResponses.Length)],
            1 => ResponseTexts.MediumResponses[random.Next(ResponseTexts.MediumResponses.Length)],
            2 => ResponseTexts.LongResponses[random.Next(ResponseTexts.LongResponses.Length)],
            _ => "Błąd w generowaniu odpowiedzi."
        };
    }
}