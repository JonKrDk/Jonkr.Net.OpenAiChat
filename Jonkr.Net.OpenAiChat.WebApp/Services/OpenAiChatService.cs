using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;
using OpenAI.Chat;

namespace Jonkr.Net.OpenAiChat.WebApp.Services;

public class OpenAiOptions
{
    public string ApiKey { get; set; } = "";
    public string Model { get; set; } = "gpt-4o-mini";
    public string SystemPrompt { get; set; } = "You are a helpful assistant.";
}

public enum ChatRole
{
    User,
    Assistant
}

public record ChatTurn(ChatRole Role, string Content);

public class OpenAiChatService
{
    private readonly ChatClient _client;
    private readonly string _systemPrompt;

    public OpenAiChatService(IOptions<OpenAiOptions> options)
    {
        var o = options.Value;
        if (string.IsNullOrWhiteSpace(o.ApiKey))
        {
            throw new InvalidOperationException(
                "OpenAI:ApiKey is not configured. Set it with: dotnet user-secrets set \"OpenAI:ApiKey\" \"<key>\"");
        }

        _client = new ChatClient(o.Model, o.ApiKey);
        _systemPrompt = o.SystemPrompt;
    }

    public async IAsyncEnumerable<string> StreamReplyAsync(
        IEnumerable<ChatTurn> history,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var messages = new List<ChatMessage> { new SystemChatMessage(_systemPrompt) };
        foreach (var turn in history)
        {
            messages.Add(turn.Role == ChatRole.User
                ? new UserChatMessage(turn.Content)
                : new AssistantChatMessage(turn.Content));
        }

        await foreach (var update in _client.CompleteChatStreamingAsync(messages, cancellationToken: cancellationToken))
        {
            foreach (var part in update.ContentUpdate)
            {
                if (!string.IsNullOrEmpty(part.Text))
                {
                    yield return part.Text;
                }
            }
        }
    }
}
