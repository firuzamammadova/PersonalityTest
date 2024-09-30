using PersonalityTest.Domain.Entities;

namespace PersonalityTest.Application.Common.Interfaces;
public interface IApplicationDbContext
{


    DbSet<Question> Questions { get; }
    DbSet<Answer> Answers { get; }
    DbSet<UserAnswer> UserAnswers { get; }
    DbSet<PersonalityTestResult> PersonalityTestResults { get; }



    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
