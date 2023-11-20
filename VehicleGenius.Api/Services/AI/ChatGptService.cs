using OpenAI;
using OpenAI.Managers;
using OpenAI.ObjectModels.RequestModels;
using VehicleGenius.Api.Dtos;
using VehicleGenius.Api.Services.SummaryTemplates;

namespace VehicleGenius.Api.Services.AI;

class ChatGptService : IAiService
{
  private readonly ISummaryTemplateService _summaryTemplateService;
  private readonly OpenAIService _openAi;

  public ChatGptService(IConfiguration configuration, ISummaryTemplateService summaryTemplateService)
  {
    _summaryTemplateService = summaryTemplateService;
    _openAi = new OpenAIService(new OpenAiOptions()
    {
      ApiKey = configuration.GetValue<string>("OpenAI:ApiKey")!,
    });
  }

  public async Task<List<ChatMessageDto>> GetAnswer(GetAnswerRequest request)
  {
    var summaryTemplate = await _summaryTemplateService.GetForVersionAsync(1, CancellationToken.None);
    var userPrompt = summaryTemplate.PromptTemplate
      .Replace("{Data}", request.Data.ToString())
      .Replace("{UserMessage}", request.Messages.Last(m => m.Role == "user").Content);

    var chatCompletionCreateRequest = new ChatCompletionCreateRequest()
    {
      Messages = new List<ChatMessage>()
      {
        ChatMessage.FromSystem(summaryTemplate.SystemPrompt),
        ChatMessage.FromUser(userPrompt),
      },
      Model = OpenAI.ObjectModels.Models.Gpt_4,
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
      throw new Exception($"ChatGPT failed to parse prompt. Code: {completionResult.Error.Code}, Message: {completionResult.Error.Message}");
    }

    var responseContent = completionResult.Choices.First().Message.Content;
    return responseContent;
  }
}
