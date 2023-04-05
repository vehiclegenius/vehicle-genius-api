using Newtonsoft.Json;
using OpenAI.GPT3;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels.RequestModels;
using VehicleGenius.Api.Models.Entities;

namespace VehicleGenius.Api.Services.AI;

class ChatGptService : IAiService
{
  private readonly OpenAIService _openAi;

  private readonly string _topicQueryPromptSystemTemplate =
    @"You are a pattern matching machine. You understand what topic the user is trying to get an answer to. You analyze data and make educated guesses. When you lack enough data you make assumptions. Don't say you are unsure or unclear, speak authoritatively.

Don't use any words to share the result, only return the UUID before the colon.";
  private readonly List<QueryTopic> _queryTopics = new()
  {
    new()
    {
      Id = Guid.NewGuid(),
      AiMatchingDescription = "The worth of the car in the future or other ownership cost topics.",
      Api = QueryTopicApi.VinAuditOwnershipCost,
    },
    new()
    {
      Id = Guid.NewGuid(),
      AiMatchingDescription = "The market value of the car.",
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

  private readonly string _answerPromptSystemTemplate =
    @"You are a helpful assistant. You analyze data and make educated guesses. When you lack enough data you make assumptions. Don't say you are unsure or unclear, speak authoritatively.

Assume we always want two different answers, for city conditions and for highway conditions.

Answer in tweet length.";

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
    var completionResult = await _openAi.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
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
    });

    if (!completionResult.Successful)
    {
      throw new Exception("ChatGPT failed to parse prompt.");
    }

    var queryTopicUuid = Guid.Parse(completionResult.Choices.First().Message.Content);
    return _queryTopics.First(qt => qt.Id == queryTopicUuid).Api;
  }

  public async Task<string> GetAnswer(object data, string prompt)
  {
    var promptWithData = @$"With the following data:

{JsonConvert.SerializeObject(data)}

Answer the following question:

{prompt}";
    
    var completionResult = await _openAi.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest()
    {
      Messages = new List<ChatMessage>()
      {
        new("system", _answerPromptSystemTemplate),
        new("user", promptWithData),
      },
      Model = "gpt-3.5-turbo",
      Temperature = (float)0.5,
      TopP = (float)0.5,
      PresencePenalty = 0,
      FrequencyPenalty = 0,
    });
    
    if (!completionResult.Successful)
    {
      throw new Exception("ChatGPT failed to parse prompt.");
    }
    
    return completionResult.Choices.First().Message.Content;
  }
}
