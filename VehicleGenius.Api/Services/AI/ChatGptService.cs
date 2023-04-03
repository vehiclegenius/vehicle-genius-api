using OpenAI.GPT3;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels.RequestModels;

namespace VehicleGenius.Api.Services.AI;

class ChatGptService : IAiService
{
  private readonly OpenAIService _openAi;

  public ChatGptService(IConfiguration configuration)
  {
    _openAi = new OpenAIService(new OpenAiOptions()
    {
      ApiKey = configuration.GetValue<string>("OpenAI:ApiKey")!,
    });
  }

  public async Task<UserPromptParts> ParsePromptIntoParts(string prompt)
  {
    var completionResult = await _openAi.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest()
    {
      Messages = new List<ChatMessage>()
      {
        new("System", "You are a helpful assistant."),
        new("User", prompt),
      },
      Model = "gpt-3.5-turbo",
    });

    if (!completionResult.Successful)
    {
      throw new Exception("ChatGPT failed to parse prompt.");
    }

    var result = completionResult.Choices.First().Message.Content;
    return new UserPromptParts()
    {
      // TODO
    };
  }

  public Task<string> BuildAnswer(object vehicle)
  {
    throw new NotImplementedException();
  }
}
