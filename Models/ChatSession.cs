using System.Text.Json.Serialization;

namespace ChatbotAPI.Models;

public class ChatSession
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [JsonIgnore]
    public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
}