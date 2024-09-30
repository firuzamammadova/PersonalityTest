using System.Text.Json.Serialization;
using System.Text.Json;
using PersonalityTest.Application.Personality.Queries;
using PersonalityTest.Domain.Entities;

namespace PersonalityTest.Web.Endpoints;

public class PersonalityTest : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
                       .MapGet(GetQuestionsAndAnswers)
                       .MapPost(SubmitTestAnswers);
    }

    public Task<List<Question>> GetQuestionsAndAnswers(ISender sender)
    {
        var result= sender.Send(new GetQuestionsAndAnswersQuery());
        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve
        };

        string json = JsonSerializer.Serialize(result, options);
        return result;
    }

    public Task<PersonalityTestResult> SubmitTestAnswers(ISender sender, SubmitTestGetResultQuery command)
    {
        return sender.Send(command);
    }
}
