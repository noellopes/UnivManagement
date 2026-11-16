using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AccommodationManagement;

public class ApplicationStatusHistory
{
    public int ApplicationStatusHistoryId { get; set; }

    [Required(ErrorMessage = "The application is required.")]
    [Display(Name = "Application")]
    public int AccommodationApplicationId { get; set; }

    [Display(Name = "Application")]
    public AccommodationApplication? AccommodationApplication { get; set; }

    [Required(ErrorMessage = "The status is required.")]
    [Display(Name = "Status")]
    public ApplicationStatus Status { get; set; }

    [Display(Name = "Changed At")]
    public DateTime ChangedAt { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "The Changed By field is required.")]
    [StringLength(100, ErrorMessage = "The {0} cannot be longer than {1} characters.")]
    [Display(Name = "Changed By")]
    public string ChangedBy { get; set; } = string.Empty;

    [StringLength(250, ErrorMessage = "The {0} cannot be longer than {1} characters.")]
    [Display(Name = "Notes")]
    public string? Notes { get; set; }
}