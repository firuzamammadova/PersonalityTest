using NUnit.Framework;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PersonalityTest.Application.Personality.Queries;
using PersonalityTest.Domain.Entities;
using PersonalityTest.Infrastructure.Data;

namespace PersonalityTest.Application.FunctionalTests.PersonalityTest
{
    public class SubmitTestGetResultQueryTests
    {
        private ApplicationDbContext _context;

        [SetUp]
        public void Setup()
        {
            // Assuming _context is initialized with an in-memory database for testing
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "PersonalityTestDb")
                .Options;

            _context = new ApplicationDbContext(options);
        }

        [Test]
        public async Task ShouldRequireUserAnswers()
        {
            var handler = new SubmitTestGetResultQueryHandler(_context);

            var query = new SubmitTestGetResultQuery
            {
                UserAnswers = new List<UserAnswer>()
            };

            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);

            await act.Should().ThrowAsync<ArgumentException>().WithMessage("No answers provided.");
        }

        [Test]
        public async Task ShouldThrowIfQuestionNotFound()
        {
            var handler = new SubmitTestGetResultQueryHandler(_context);

            var query = new SubmitTestGetResultQuery
            {
                UserAnswers = new List<UserAnswer>
                {
                    new UserAnswer { QuestionId = 999, Answer = new Answer { AnswerText = "Sample Answer" } }
                }
            };

            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);

            await act.Should().ThrowAsync<ArgumentException>().WithMessage("Question with ID 999 not found.");
        }

        [Test]
        public async Task ShouldReturnCorrectPersonalityResult()
        {
            // Add a sample question to the in-memory database
            var question = new Question
            {
                Answers = new List<Answer>
                {
                    new Answer { AnswerText = "Option 1" },
                    new Answer { AnswerText = "Option 2" }
                }
            };
            await _context.Questions.AddAsync(question);
            await _context.SaveChangesAsync();

            var handler = new SubmitTestGetResultQueryHandler(_context);

            var query = new SubmitTestGetResultQuery
            {
                UserAnswers = new List<UserAnswer>
                {
                    new UserAnswer { QuestionId = question.Id, Answer = new Answer { AnswerText = "Option 1" } }
                }
            };

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.PersonalityTrait.Should().Be("Introvert");
            result.TestTakenOn.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
        }

        [Test]
        public async Task ShouldSaveResultToDatabase()
        {
            // Add a sample question to the in-memory database
            var question = new Question
            {
                Answers = new List<Answer>
                {
                    new Answer { AnswerText = "Option 1" },
                    new Answer { AnswerText = "Option 2" }
                }
            };
            await _context.Questions.AddAsync(question);
            await _context.SaveChangesAsync();

            var handler = new SubmitTestGetResultQueryHandler(_context);

            var query = new SubmitTestGetResultQuery
            {
                UserAnswers = new List<UserAnswer>
                {
                    new UserAnswer { QuestionId = question.Id, Answer = new Answer { AnswerText = "Option 1" } }
                }
            };

            var result = await handler.Handle(query, CancellationToken.None);

            // Check that the result was saved in the database
            var savedResult = await _context.PersonalityTestResults
                .FirstOrDefaultAsync(r => r.PersonalityTrait == result.PersonalityTrait);

            savedResult.Should().NotBeNull();
            savedResult?.PersonalityTrait.Should().Be(result.PersonalityTrait);
        }
        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
    }
}
