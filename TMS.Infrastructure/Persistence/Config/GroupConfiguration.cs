using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TMS.Domain.Common.Constrains;
using TMS.Domain.Groups;

namespace TMS.Infrastructure.Persistence.Config;

public class GroupConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.HasKey(c => c.Id);
        builder.ToTable("Groups");
        builder.Property(t => t.Id).HasConversion(
            v => v.Value,
            v => GroupId.Create(v)
        );
        builder.Property(c => c.Name).HasMaxLength(Constrains.Group.Name.Max).IsRequired();
        builder.Property(t => t.Grade).HasConversion(
            v => v.ToString(),
            v => Enum.Parse<Grade>(v)
        );
        
        
        builder.HasMany(t => t.Sessions)
            .WithOne()
            .HasForeignKey(a => a.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(g => g.Accounts)
            .WithOne()
            .HasForeignKey(a => a.GroupId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasIndex(c => new {c.TeacherId,c.Name,c.Grade} ).IsUnique();
    }
}