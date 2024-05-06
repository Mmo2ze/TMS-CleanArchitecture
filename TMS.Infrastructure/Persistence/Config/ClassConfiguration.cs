using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TMS.Domain.Groups;

namespace TMS.Infrastructure.Persistence.Config;

public class ClassConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.HasKey(c => c.Id);
        builder.ToTable("Groups");
        builder.Property(t => t.Id).HasConversion(
            v => v.Value,
            v => GroupId.Create(v)
        );
        builder.Property(c => c.Name).HasMaxLength(35).IsRequired();
        builder.Property(t => t.Grade).HasConversion(
            v => v.ToString(),
            v => Enum.Parse<Grade>(v)
        );
        
        
        builder.HasMany(t => t.Sessions)
            .WithOne()
            .HasForeignKey(a => a.GroupId);
        builder.HasMany(c => c.Students)
            .WithMany(s => s.Classes);
        
        builder.HasIndex(c => new {c.TeacherId,c.Name,c.Grade} ).IsUnique();
    }
}