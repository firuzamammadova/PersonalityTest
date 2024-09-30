using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PersonalityTest.Application.Personality.Queries;
using PersonalityTest.Domain.Entities;
using PersonalityTest.Infrastructure.Data;

namespace PersonalityTest.Application.FunctionalTests.PersonalityTest;
public class GetQuestionsAndAnswersQueryTests
{
    private ApplicationDbContext _context;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "PersonalityTestDb")
            .Options;

        _context = new ApplicationDbContext(options);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task ShouldReturnListOfQuestionsAndAnswers()
    {
        // Arrange
        var question1 = new Question
        {
            Id = 1,
            Answers = new List<Answer>
                {
                    new Answer { AnswerText = "Answer 1" },
                    new Answer { AnswerText = "Answer 2" }
                }
        };

        var question2 = new Question
        {
            Id = 2,
            Answers = new List<Answer>
                {
                    new Answer { AnswerText = "Answer 3" },
                    new Answer { AnswerText = "Answer 4" }
                }
        };

        await _context.Questions.AddRangeAsync(question1, question2);
        await _context.SaveChangesAsync();

        var handler = new GetQuestionsAndAnswersQueryandler(_context);

        var query = new GetQuestionsAndAnswersQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result[0].Id.Should().Be(question1.Id);
        result[0].Answers.Should().HaveCount(2);
        result[1].Id.Should().Be(question2.Id);
        result[1].Answers.Should().HaveCount(2);
    }

    [Test]
    public async Task ShouldReturnEmptyListWhenNoQuestionsExist()
    {
        // Arrange
        var handler = new GetQuestionsAndAnswersQueryandler(_context);
        var query = new GetQuestionsAndAnswersQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}
