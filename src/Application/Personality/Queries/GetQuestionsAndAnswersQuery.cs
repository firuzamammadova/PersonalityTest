using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalityTest.Application.Common.Interfaces;
using PersonalityTest.Domain.Entities;

namespace PersonalityTest.Application.Personality.Queries;


public record GetQuestionsAndAnswersQuery : IRequest<List<Question>>;

public class GetQuestionsAndAnswersQueryandler : IRequestHandler<GetQuestionsAndAnswersQuery, List<Question>>
{
    private readonly IApplicationDbContext _context;

    public GetQuestionsAndAnswersQueryandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Question>> Handle(GetQuestionsAndAnswersQuery request, CancellationToken cancellationToken)
    {
        var questions = await _context.Questions
            .AsNoTracking()
            .Include(q => q.Answers)
            .ToListAsync();

 

        return questions.ToList();
    }

}
