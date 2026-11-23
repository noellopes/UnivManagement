using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AccommodationManagement;

public class PossibleStatus
{
    public int PossibleStatusId { get; set; }

    [Required(ErrorMessage = "The status name is required.")]
    [StringLength(100)]
    [Display(Name = "Status")]
    public string Name { get; set; } = string.Empty;

    public ICollection<ApplicationStatusHistory>? ApplicationStatusHistories { get; set; }
}