using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TMS.Domain.Students;

namespace TMS.Infrastructure.Persistence;

public class MainContext:DbContext
{
 
	
		public MainContext(DbContextOptions<MainContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(MainContext).Assembly);
			modelBuilder.Model.GetEntityTypes()
				.SelectMany(e => e.GetProperties())
				.Where(p => p.IsPrimaryKey())
				.ToList()
				.ForEach(p => p.ValueGenerated = ValueGenerated.Never);
			base.OnModelCreating(modelBuilder);
		}
		
		public DbSet<Student> Students { get; set; }

}