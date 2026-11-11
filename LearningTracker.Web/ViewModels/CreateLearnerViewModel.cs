using System.ComponentModel.DataAnnotations;

namespace LearningTracker.Web.ViewModels;

public class CreateLearnerViewModel
{
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(150)]
    public string DisplayName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    [StringLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Cohort is required.")]
    [StringLength(100)]
    public string Cohort { get; set; } = string.Empty;
}