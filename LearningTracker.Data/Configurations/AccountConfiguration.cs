using System;
using LearningTracker.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningTracker.Data.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        // Table name and primary key
        builder.ToTable("accounts");

        builder.HasKey(a => a.Id);

        // Properties
        builder.Property(a => a.NormalizedEmail)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(a => a.PasswordHash)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(a => a.Role)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(a => a.MustChangePassword)
            .IsRequired();

        builder.Property(a => a.TemporaryPasswordExpiresAt);

        builder.Property(a => a.SecurityStamp)
            .IsRequired()
            .HasMaxLength(36);

        // Unique email constraint
        builder.HasIndex(a => a.NormalizedEmail)
            .IsUnique();

        // One Account can have at most one Learner
        // A Coordinator account does not need a Learner
        builder.HasOne(a => a.Learner)
            .WithOne(l => l.Account)
            .HasForeignKey<Learner>(l => l.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
