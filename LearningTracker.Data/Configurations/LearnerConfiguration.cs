using LearningTracker.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningTracker.Data.Configurations;

public class LearnerConfiguration : IEntityTypeConfiguration<Learner>
{
    public void Configure(EntityTypeBuilder<Learner> builder)
    {
        builder.ToTable("learners");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.DisplayName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(l => l.Cohort)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(l => l.AccountId)
            .IsRequired();

        // One Account belongs to one Learner profile
        builder.HasIndex(l => l.AccountId)
            .IsUnique();

        // One Learner can have multiple Assignments
        builder.HasMany(l => l.Assignments)
            .WithOne(a => a.Learner)
            .HasForeignKey(a => a.LearnerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}