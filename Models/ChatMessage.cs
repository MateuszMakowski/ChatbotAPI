using System.Text.Json.Serialization;

namespace ChatbotAPI.Models;

public class ChatMessage
{
    public int Id { get; set; }
    public int ChatSessionId { get; set; }
    [JsonIgnore]
    public ChatSession ChatSession { get; set; }
    public string UserMessage { get; set; }
    public string BotResponse { get; set; }
    public int? Rating { get; set; }
    public DateTime Timestamp { get; set; }
}