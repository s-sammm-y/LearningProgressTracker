using System;
using LearningTracker.Models.Enums;

namespace LearningTracker.Models.Entities;

public class Account
{
    public Guid Id { get; set; }

    public string NormalizedEmail { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public UserRole Role { get; set; }

    public bool MustChangePassword { get; set; }

    public DateTime? TemporaryPasswordExpiresAt { get; set; }

    public string SecurityStamp { get; set; } = Guid.NewGuid().ToString();

    // Navigation property
    public Learner? Learner { get; set; }
}
