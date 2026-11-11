using System;

namespace LearningTracker.Web.ViewModels;

public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public string Message { get; set; } = "Something went wrong.";

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
