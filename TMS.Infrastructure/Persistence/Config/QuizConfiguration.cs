using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TMS.Domain.Accounts;
using TMS.Domain.Quizzes;

namespace TMS.Infrastructure.Persistence.Config;

public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
{
    public void Configure(EntityTypeBuilder<Quiz> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(
            v => v.Value,
            v => new QuizId(v));
        builder.Property(x => x.Degree).IsRequired();
        builder.Property(x => x.MaxDegree).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdatedAt).IsRequired(false);

        builder.Property(x => x.AccountId);
        builder.Property(x => x.AddedById).IsRequired(false);
        builder.Property(x => x.UpdatedById).IsRequired(false);

        // Relationships
        builder.HasOne(x => x.Teacher)
            .WithMany()
            .HasForeignKey(x => x.TeacherId);


        
        builder.HasOne(x => x.AddedBy)
            .WithMany()
            .HasForeignKey(x => x.AddedById);

        builder.HasOne(x => x.UpdatedBy)
            .WithMany()
            .HasForeignKey(x => x.UpdatedById);

        builder.HasOne<Account>()
            .WithMany(x => x.Quizzes)
            .HasForeignKey(x => x.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => new { x.AccountId, x.CreatedAt }).IsUnique();
    }
}