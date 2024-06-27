using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TMS.Domain.Assistants;
using TMS.Domain.Groups;
using TMS.Domain.Holidays;
using TMS.Domain.Teachers;

namespace TMS.Infrastructure.Persistence.Config;

public class HolidayConfiguration : IEntityTypeConfiguration<Holiday>
{
    public void Configure(EntityTypeBuilder<Holiday> builder)
    {
        builder.ToTable("Holidays");
        builder.HasKey(h => h.Id);
        builder.Property(h => h.Id).HasConversion(h => h.Value, v => new HolidayId(v));

        //relations 
        builder.HasOne<Teacher>().WithMany(x => x.Holidays).HasForeignKey(h => h.TeacherId);
        builder.HasOne<Group>().WithMany().HasForeignKey(h => h.GroupId);
        builder.HasOne(x => x.CreatedBy)
            .WithMany()
            .HasForeignKey(x => x.CreatedById);

        builder.HasOne(x => x.ModifiedBy)
            .WithMany()
            .HasForeignKey(x => x.ModifiedById);
        
        //index 
        builder.HasIndex(h => new { h.TeacherId, h.StartDate, h.EndDate }).IsUnique();
    }
}