using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TMS.Domain.Admins;
using TMS.Domain.Assistants;
using TMS.Domain.Common.Models;
using TMS.Domain.OutBox;
using TMS.Domain.Parents;
using TMS.Domain.Students;
using TMS.Domain.Teachers;
using TMS.Infrastructure.Persistence.interceptors;

namespace TMS.Infrastructure.Persistence;

public class MainContext : DbContext
{
    private readonly ConvertDomainEventToOutBoxMessageInterceptor _domainEventToOutBoxMessageInterceptor;

    public MainContext(DbContextOptions<MainContext> options,
        ConvertDomainEventToOutBoxMessageInterceptor domainEventToOutBoxMessageInterceptor) : base(options)
    {
        _domainEventToOutBoxMessageInterceptor = domainEventToOutBoxMessageInterceptor;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Ignore<List<IntegrationEvent>>()
            .Ignore<List<DomainEvent>>()
            .ApplyConfigurationsFromAssembly(typeof(MainContext).Assembly);


        modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetProperties())
            .Where(p => p.IsPrimaryKey())
            .ToList()
            .ForEach(p => p.ValueGenerated = ValueGenerated.Never);
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Student> Students { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Assistant> Assistants { get; set; }
    public DbSet<Parent> Parents { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_domainEventToOutBoxMessageInterceptor);
        base.OnConfiguring(optionsBuilder);
    }
}