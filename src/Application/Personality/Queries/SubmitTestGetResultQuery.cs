using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalityTest.Application.Common.Interfaces;
using PersonalityTest.Domain.Entities;

namespace PersonalityTest.Application.Personality.Queries;


public record SubmitTestGetResultQuery : IRequest<PersonalityTestResult>
{
    public List<UserAnswer> UserAnswers { get; init; } = new List<UserAnswer>();
}

public class SubmitTestGetResultQueryHandler : IRequestHandler<SubmitTestGetResultQuery, PersonalityTestResult>
{
    private readonly IApplicationDbContext _context;

    public SubmitTestGetResultQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PersonalityTestResult> Handle(SubmitTestGetResultQuery request, CancellationToken cancellationToken)
    {
        if (request.UserAnswers == null || !request.UserAnswers.Any())
        {
            throw new ArgumentException("No answers provided.");
        }
        int score = 0;

        foreach (var userAnswer in request.UserAnswers)
        {
            var question = _context.Questions
                .Include(q => q.Answers)
                .AsNoTracking()
                .FirstOrDefault(q => q.Id == userAnswer.QuestionId);
            if (question == null)
            {
                throw new ArgumentException($"Question with ID {userAnswer.QuestionId} not found.");
            }
            var selectedAnswer = question?.Answers
                .FirstOrDefault(a => a.AnswerText!.Equals(userAnswer?.Answer?.AnswerText, StringComparison.OrdinalIgnoreCase));

            if (selectedAnswer != null)
            {
                int answerIndex = question!.Answers.IndexOf(selectedAnswer);
                score += answerIndex + 1;
            }
        }
        var personalityTrait = (score > (request.UserAnswers.Count * 2.5)) ? "Extrovert" : "Introvert";

        var result = new PersonalityTestResult
        {
            PersonalityTrait = personalityTrait,
            TestTakenOn = DateTime.Now
        };
        try
        {

            await _context.PersonalityTestResults.AddAsync(result);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {

            throw new InvalidOperationException("Error saving the test result.", ex);
        }



        return result;
    }

}
