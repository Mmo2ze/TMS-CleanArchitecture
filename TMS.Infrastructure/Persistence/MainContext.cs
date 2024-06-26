using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TMS.Domain.Accounts;
using TMS.Domain.Admins;
using TMS.Domain.Assistants;
using TMS.Domain.Attendances;
using TMS.Domain.Common.Models;
using TMS.Domain.Groups;
using TMS.Domain.Parents;
using TMS.Domain.Quizzes;
using TMS.Domain.RefreshTokens;
using TMS.Domain.Schedulers;
using TMS.Domain.Sessions;
using TMS.Domain.Students;
using TMS.Domain.Teachers;
using TMS.Infrastructure.Persistence.Interceptors;

namespace TMS.Infrastructure.Persistence;

public class MainContext :DbContext
{

    private readonly PublishDomainEventsInterceptor _publishDomainEventsInterceptor;
    public MainContext(DbContextOptions< MainContext> options, PublishDomainEventsInterceptor publishDomainEventsInterceptor) : base(options)
    {
        _publishDomainEventsInterceptor = publishDomainEventsInterceptor;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder
            .Ignore<List<DomainEvent>>()
            .ApplyConfigurationsFromAssembly(typeof(MainContext).Assembly);


        modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetProperties())
            .Where(p => p.IsPrimaryKey())
            .ToList()
            .ForEach(p => p.ValueGenerated = ValueGenerated.Never);
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_publishDomainEventsInterceptor);
        base.OnConfiguring(optionsBuilder);
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
    public DbSet<Attendance> Attendances { get; set; }
    public DbSet<Scheduler> AttendanceSchedulers { get; set; }
}