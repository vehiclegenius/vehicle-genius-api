using Newtonsoft.Json;
using OpenAI.GPT3;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels.RequestModels;
using VehicleGenius.Api.Models.Entities;
using VehicleGenius.Api.Services.VinAudit;

namespace VehicleGenius.Api.Services.AI;

class ChatGptService : IAiService
{
  private readonly OpenAIService _openAi;

  private readonly string _summaryPromptSystem =
    "You are a helpful assistant. You take JSON and transform it into a digestible list of data. You don't omit any numbers. Money amounts are in dollars. The prompt may contain further hints.";

  private readonly string _topicQueryPromptSystemTemplate =
    @"You are a helpful assistant. You help me figure out to what possible topic the sentence/question could be related to. Don't use any words to share the result, only return the UUID before the colon.";
  private readonly List<QueryTopic> _queryTopics = new()
  {
    new()
    {
      Id = Guid.NewGuid(),
      AiMatchingDescription = "The worth of the car in the future or other ownership cost topics like depreciation cost, insurance cost, fuel cost, maintenance cost, repairs cost, fees cost, and total costs.",
      Api = QueryTopicApi.VinAuditOwnershipCost,
    },
    new()
    {
      Id = Guid.NewGuid(),
      AiMatchingDescription = "The market value of the car in present time and other data related to the price of the car.",
      Api = QueryTopicApi.VinAuditMarketValue,
    },
    new()
    {
      Id = Guid.NewGuid(),
      AiMatchingDescription = "The specifications of the car, its engine, fuel usage, etc.",
      Api = QueryTopicApi.VinAuditSpecifications,
    },
    new()
    {
      Id = Guid.NewGuid(),
      AiMatchingDescription = "None of the above.",
      Api = QueryTopicApi.None,
    },
  };

  private readonly string _answerPromptSystemTemplate = "You are a helpful assistant.";

  public ChatGptService(IConfiguration configuration)
  {
    _openAi = new OpenAIService(new OpenAiOptions()
    {
      ApiKey = configuration.GetValue<string>("OpenAI:ApiKey")!,
    });
  }

  public async Task<QueryTopicApi> GetQueryTopicApi(string prompt)
  {
    var topics = _queryTopics.Select(qt => $"{qt.Id}: {qt.AiMatchingDescription}");
    var promptTopics = string.Join("\n", topics);
    var promptWithTopics = 
        $@"Topics:

{promptTopics}

Sentence:

{prompt}";
    var chatCompletionCreateRequest = new ChatCompletionCreateRequest
    {
      Messages = new List<ChatMessage>()
      {
        new("system", _topicQueryPromptSystemTemplate),
        new("user", promptWithTopics),
      },
      Model = "gpt-3.5-turbo",
      Temperature = (float)0,
      TopP = (float)0.5,
      PresencePenalty = 0,
      FrequencyPenalty = 0,
    };
    var responseContent = await ResponseContent(chatCompletionCreateRequest);
    var queryTopicUuid = Guid.Parse(responseContent);
    return _queryTopics.First(qt => qt.Id == queryTopicUuid).Api;
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

  public async Task<string> GetAnswer(GetAnswerRequest request)
  {
    var pastOrFuture = request.DataInFuture ? "of the future" : "of the past";
    var promptWithData = @$"With this data:

{request.Data}

{request.Prompt}";

    var chatCompletionCreateRequest = new ChatCompletionCreateRequest()
    {
      Messages = new List<ChatMessage>()
      {
        new("system", _answerPromptSystemTemplate),
        new("user", promptWithData),
      },
      Model = "gpt-3.5-turbo",
      Temperature = (float)0,
      TopP = (float)0.95,
      PresencePenalty = 0,
      FrequencyPenalty = 0,
      MaxTokens = 512,
    };
    var responseContent = await ResponseContent(chatCompletionCreateRequest);
    return responseContent;
  }

  public async Task<string> SummarizeVehicleData(VinAuditData vehicleData)
  {
    var prompts = new List<ChatCompletionCreateRequest>()
    {
      new()
      {
        Messages = new List<ChatMessage>()
        {
          new("system", _summaryPromptSystem),
          new("user", $"Current vehicle specifications:\n\n{JsonConvert.SerializeObject(vehicleData.Specifications)}"),
        },
        Model = "gpt-3.5-turbo",
        Temperature = (float)0.7,
        TopP = (float)1,
        PresencePenalty = 0,
        FrequencyPenalty = 0,
      },
      new()
      {
        Messages = new List<ChatMessage>()
        {
          new("system", _summaryPromptSystem),
          new("user", $"Current vehicle market value data:\n\n{JsonConvert.SerializeObject(vehicleData.MarketValue)}"),
        },
        Model = "gpt-3.5-turbo",
        Temperature = (float)0.7,
        TopP = (float)1,
        PresencePenalty = 0,
        FrequencyPenalty = 0,
      },
      new()
      {
        Messages = new List<ChatMessage>()
        {
          new("system", _summaryPromptSystem),
          new("user", $"Future vehicle costs associated with various expenses, costs are in USD and represent a year on year cost:\n\n{JsonConvert.SerializeObject(vehicleData.OwnershipCost)}"),
        },
        Model = "gpt-3.5-turbo",
        Temperature = (float)0.7,
        TopP = (float)1,
        PresencePenalty = 0,
        FrequencyPenalty = 0,
      }
    };

    var result = "";

    foreach (var chatCompletionCreateRequest in prompts)
    {
      result += await ResponseContent(chatCompletionCreateRequest);
    }

    return result;
  }
}
