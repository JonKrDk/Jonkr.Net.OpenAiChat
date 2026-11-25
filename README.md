# Jonkr.Net.OpenAiChat

A small Blazor web app that provides a chat interface for OpenAI models. Replies are streamed to the page token by token as they are generated.

## Features

- Chat page at `/chat` with user and assistant message bubbles
- Streaming responses from the OpenAI Chat Completions API
- Full conversation history is sent with each request, so the model keeps context
- Enter sends, Shift+Enter inserts a new line
- **Stop** button to cancel a response in progress, and **New chat** to start over
- Auto-scroll to the latest message
- API errors are shown in the page instead of crashing the circuit

## Tech stack

- .NET 10, Blazor Web App with Interactive Server rendering
- [OpenAI .NET library](https://github.com/openai/openai-dotnet) (`OpenAI` NuGet package)

## Getting started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- An [OpenAI API key](https://platform.openai.com/api-keys)

### Configure the API key

The API key is read from the `OpenAI:ApiKey` configuration setting. Store it with [.NET user secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets) so it never ends up in source control:

```powershell
cd Jonkr.Net.OpenAiChat.WebApp
dotnet user-secrets set "OpenAI:ApiKey" "<your-api-key>"
```

### Run

```powershell
dotnet run --project Jonkr.Net.OpenAiChat.WebApp
```

Then open the URL shown in the console (for example `http://localhost:5293`) and go to **Chat** in the navigation menu.

## Configuration

| Setting | Default | Description |
|---|---|---|
| `OpenAI:ApiKey` | *(required)* | Your OpenAI API key. Keep it in user secrets or environment variables. |
| `OpenAI:Model` | `gpt-4o-mini` | The chat model to use. |
| `OpenAI:SystemPrompt` | `You are a helpful assistant.` | System message sent at the start of every conversation. |

Standard ASP.NET Core configuration sources apply, so settings can also be supplied through `appsettings.json` or environment variables (for example `OpenAI__ApiKey`).

## Project structure

```
Jonkr.Net.OpenAiChat.WebApp/
├── Components/
│   ├── Layout/            Main layout and navigation menu
│   └── Pages/
│       └── Chat.razor     Chat UI (plus .css and .js for styling and scroll/Enter handling)
├── Services/
│   └── OpenAiChatService.cs   Wraps the OpenAI client and streams replies
└── Program.cs             App startup and service registration
```

## Notes

- Conversation history lives in memory for the duration of the Blazor circuit; it is not persisted, and reloading the page starts a new chat.
- The app uses a single shared API key for all users, so don't expose it publicly without adding authentication and usage limits.
