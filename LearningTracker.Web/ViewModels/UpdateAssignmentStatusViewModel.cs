using System.ComponentModel.DataAnnotations;
using LearningTracker.Models.Enums;

namespace LearningTracker.Web.ViewModels;

public class UpdateAssignmentStatusViewModel
{
    [Required]
    public Guid AssignmentId { get; set; }

    [Required]
    public AssignmentStatus Status { get; set; }
}