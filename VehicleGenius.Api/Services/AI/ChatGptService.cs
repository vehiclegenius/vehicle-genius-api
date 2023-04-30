using OpenAI.GPT3;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels.RequestModels;
using VehicleGenius.Api.Dtos;

namespace VehicleGenius.Api.Services.AI;

class ChatGptService : IAiService
{
  private readonly OpenAIService _openAi;

  private readonly string _summaryPromptSystem =
    "You are a helpful assistant. You take JSON and transform it into a digestible list of data. You don't omit any numbers. Money amounts are in dollars. The prompt may contain further hints.";

  private readonly string _answerPromptSystemTemplate = "You are a helpful assistant.";

  public ChatGptService(IConfiguration configuration)
  {
    _openAi = new OpenAIService(new OpenAiOptions()
    {
      ApiKey = configuration.GetValue<string>("OpenAI:ApiKey")!,
    });
  }

  public async Task<List<ChatMessageDto>> GetAnswer(GetAnswerRequest request)
  {
    var promptWithData = @$"With this data:

{request.Data}

{request.Messages.Last(m => m.Role == "user").Content}";

    var chatCompletionCreateRequest = new ChatCompletionCreateRequest()
    {
      Messages = new List<ChatMessage>()
      {
        ChatMessage.FromSystem(_answerPromptSystemTemplate),
        ChatMessage.FromUser(promptWithData),
      },
      Model = "gpt-3.5-turbo",
      Temperature = (float)0,
      TopP = (float)0.95,
      PresencePenalty = 0,
      FrequencyPenalty = 0,
      MaxTokens = 512,
    };
    var response = await ResponseContent(chatCompletionCreateRequest);
    var responseMessages = request.Messages.Concat(new[]
    {
      new ChatMessageDto()
      {
        Content = response,
        Role = "assistant",
      },
    }).ToList();
    return responseMessages;
  }

  private async Task<string> ResponseContent(ChatCompletionCreateRequest chatCompletionCreateRequest)
  {
    var completionResult = await _openAi.ChatCompletion.CreateCompletion(chatCompletionCreateRequest);

    if (!completionResult.Successful)
    {
      throw new Exception("ChatGPT failed to parse prompt.");
    }

    var responseContent = completionResult.Choices.First().Message.Content;
    return responseContent;
  }
}
