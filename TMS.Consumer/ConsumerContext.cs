using MassTransit;
using Microsoft.EntityFrameworkCore;
using TMS.Domain.Account;
using TMS.Domain.Admins;
using TMS.Domain.Assistants;
using TMS.Domain.Groups;
using TMS.Domain.Parents;
using TMS.Domain.Quizzes;
using TMS.Domain.RefreshTokens;
using TMS.Domain.Sessions;
using TMS.Domain.Students;
using TMS.Domain.Teachers;

namespace TMS.Consumer;

public class ConsumerContext : DbContext
{
    public ConsumerContext(DbContextOptions<ConsumerContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }
    
    public DbSet<Student> Students { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Assistant> Assistants { get; set; }
    public DbSet<Parent> Parents { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Quiz> Quizzes { get; set; }
}