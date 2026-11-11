using LearningTracker.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningTracker.Data.Configurations;

public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
{
    public void Configure(EntityTypeBuilder<Activity> builder)
    {
        builder.ToTable("activities");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.Topic)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(a => a.PlannedHours)
            .IsRequired()
            .HasPrecision(6, 2);

        // Planned hours must be greater than zero
        builder.ToTable(t =>
            t.HasCheckConstraint(
                "ck_activities_planned_hours",
                "\"PlannedHours\" > 0"
            ));

        // One Activity can have multiple Assignments
        builder.HasMany(a => a.Assignments)
            .WithOne(a => a.Activity)
            .HasForeignKey(a => a.ActivityId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}