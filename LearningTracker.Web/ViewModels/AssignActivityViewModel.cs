using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LearningTracker.Web.ViewModels;

public class AssignActivityViewModel
{
    [Required(ErrorMessage = "Please select a candidate.")]
    public Guid LearnerId { get; set; }

    [Required(ErrorMessage = "Please select an activity.")]
    public Guid ActivityId { get; set; }

    public List<SelectListItem> Learners { get; set; } = new();

    public List<SelectListItem> Activities { get; set; } = new();
}