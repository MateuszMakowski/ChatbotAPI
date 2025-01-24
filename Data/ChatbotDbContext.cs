using ChatbotAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatbotAPI.Data;

public class ChatbotDbContext : DbContext
{
    public DbSet<ChatMessage> ChatMessages { get; set; }
    
    public ChatbotDbContext(DbContextOptions<ChatbotDbContext> options) : base(options) {}
}