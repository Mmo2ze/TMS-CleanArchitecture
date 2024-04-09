using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TMS.Domain.Admins;
using TMS.Domain.Assistants;
using TMS.Domain.Parents;
using TMS.Domain.Students;
using TMS.Domain.Teachers;

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
		public DbSet<Admin> Admins { get; set; }
		public DbSet<Teacher> Teachers { get; set; }
		public DbSet<Assistant> Assistants { get; set; }
		public DbSet<Parent> Parents { get; set; }

}