using System.ComponentModel.DataAnnotations;

namespace LearningTracker.Web.ViewModels;

public class EditActivityViewModel
{
    [Required]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Topic is required.")]
    [StringLength(150)]
    public string Topic { get; set; } = string.Empty;

    [Required(ErrorMessage = "Planned hours are required.")]
    [Range(0.01, 9999.99,
        ErrorMessage = "Planned hours must be greater than zero.")]
    public decimal PlannedHours { get; set; }
}