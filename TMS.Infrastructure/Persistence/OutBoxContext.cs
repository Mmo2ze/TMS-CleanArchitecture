using Microsoft.EntityFrameworkCore;
using TMS.Domain.Common.Models;
using TMS.Infrastructure.Persistence.interceptors;

namespace TMS.Infrastructure.Persistence;

public class OutBoxContext : DbContext
{
    private readonly ConvertDomainEventToOutBoxMessageInterceptor _domainEventToOutBoxMessageInterceptor;

    public OutBoxContext(DbContextOptions<MainContext> options,
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

        base.OnModelCreating(modelBuilder);
    }

  


}