using ChatbotAPI.Data;
using ChatbotAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Microsoft.AspNetCore.Components.Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly ChatbotDbContext _context;

    public ChatController(ChatbotDbContext context)
    {
        _context = context;
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetChatHistory()
    {
        var messages = await _context.ChatMessages.OrderBy(m => m.Timestamp).ToListAsync();
        return Ok(messages);
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] string userMessage)
    {
        var botResponse = GenerateResponse(userMessage);

        var message = new ChatMessage
        {
            UserMessage = userMessage,
            BotResponse = botResponse
        };
        
        _context.ChatMessages.Add(message);
        await _context.SaveChangesAsync();

        return Ok(message);
    }
    
    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateMessage(int id, [FromBody] string botResponse)
    {
        var message = await _context.ChatMessages.FindAsync(id);
        if (message == null)
        {
            return NotFound(); 
        }
        
        message.BotResponse = botResponse;
        await _context.SaveChangesAsync(); 

        return NoContent(); 
    }

    [HttpPut("rate/{id}")]
    public async Task<IActionResult> RateMessage(int id, [FromBody] int rating)
    {
        var message = await _context.ChatMessages.FindAsync(id);
        if (message == null) return NotFound();

        message.Rating = rating;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private string GenerateResponse(string userMessage)
{
    var shortResponses = new[]
    {
        "Lorem ipsum dolor sit amet.",
        "Proin vel urna at arcu pharetra volutpat.",
        "Aliquam eget est vel justo elementum accumsan.",
        "Dziękuję za wiadomość!",
        "To bardzo ciekawe pytanie."
    };
    
    var mediumResponses = new[]
    {
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin vel urna at arcu pharetra volutpat. Sed tincidunt massa ut purus elementum fringilla.",
        "Vivamus sollicitudin libero id nulla vestibulum, in rutrum enim facilisis. Praesent ac nisi eu mi sagittis vehicula a id metus. Etiam a tortor id nisl vulputate scelerisque.",
        "Fusce convallis, orci nec eleifend pharetra, lorem urna facilisis velit, sed pharetra nulla felis eget justo. Integer posuere nisi eget justo scelerisque convallis. Nam non orci ut mi tempus feugiat.",
        "To bardzo interesujące pytanie, które warto zgłębić. Wiele osób zadaje sobie podobne pytania, starając się znaleźć najlepsze rozwiązanie.",
        "Z pewnością warto zastanowić się nad tym tematem w szerszym kontekście. Istnieje wiele aspektów, które mogą wpływać na odpowiedź."
    };
    
    var longResponses = new[]
    {
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin vel urna at arcu pharetra volutpat. Sed tincidunt massa ut purus elementum fringilla. Vivamus sollicitudin libero id nulla vestibulum, in rutrum enim facilisis. Praesent ac nisi eu mi sagittis vehicula a id metus. Etiam a tortor id nisl vulputate scelerisque. Fusce convallis, orci nec eleifend pharetra, lorem urna facilisis velit, sed pharetra nulla felis eget justo.",
        "Aliquam eget est vel justo elementum accumsan. Integer posuere nisi eget justo scelerisque convallis. Nam non orci ut mi tempus feugiat. Curabitur in dictum augue. Nam eget sapien vitae enim venenatis dictum at a arcu. Pellentesque id justo ac lectus suscipit condimentum. Vivamus scelerisque sapien sit amet ligula tempor, et fermentum erat scelerisque. Phasellus sollicitudin convallis justo, id volutpat ligula luctus vitae. Etiam suscipit metus sed ligula ultricies, a elementum augue dapibus.",
        "Dziękuję za Twoje pytanie. To bardzo ciekawy temat, który wymaga szczegółowej analizy. Wiele osób zadaje sobie podobne pytania, starając się zrozumieć, jak najlepiej podejść do danego zagadnienia. Istnieje wiele różnych perspektyw, które mogą pomóc w lepszym zrozumieniu problemu. Warto również rozważyć różne aspekty tego zagadnienia, aby móc podejść do niego kompleksowo i znaleźć najbardziej odpowiednie rozwiązanie. Jeśli chcesz zgłębić ten temat bardziej szczegółowo, chętnie podam więcej informacji.",
        "To złożony temat, który można rozważać z różnych perspektyw. W kontekście tego zagadnienia warto zwrócić uwagę na różnorodne aspekty, które mogą wpływać na wynik. Na przykład, czynniki takie jak warunki początkowe, różnorodność podejść, a także potencjalne ryzyka i korzyści. Rozważając temat w szerszym kontekście, można zauważyć, że każde rozwiązanie niesie ze sobą zarówno pozytywne, jak i negatywne skutki. Dlatego warto podchodzić do tego tematu z otwartym umysłem i być gotowym na eksplorację różnych opcji."
    };

    
    var random = new Random();
    int category = random.Next(0, 3); 

    switch (category)
    {
        case 0: 
            return shortResponses[random.Next(shortResponses.Length)];
        case 1: 
            return mediumResponses[random.Next(mediumResponses.Length)];
        case 2:
            return longResponses[random.Next(longResponses.Length)];
        default:
            return "Błąd w generowaniu odpowiedzi.";
    }
}
}
