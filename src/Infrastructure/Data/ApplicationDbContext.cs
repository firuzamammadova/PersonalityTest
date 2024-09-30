using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PersonalityTest.Application.Common.Interfaces;
using PersonalityTest.Domain.Entities;
using PersonalityTest.Infrastructure.Identity;

namespace PersonalityTest.Infrastructure.Data;
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Question> Questions => Set<Question>();
    public DbSet<Answer> Answers => Set<Answer>();

    public DbSet<UserAnswer> UserAnswers => Set<UserAnswer>();
    public DbSet<PersonalityTestResult> PersonalityTestResults => Set<PersonalityTestResult>();




    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
