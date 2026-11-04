using System.ComponentModel.DataAnnotations;
namespace UnivManagement.Models.AccommodationManagement;

public class Residence
{
    public int ResidenceId { get; set; }

    [Required(ErrorMessage = "The {0} is required.")]
    [StringLength(60, ErrorMessage = "The {0} cannot be longer than {1} characters.")]
    [Display(Name = "Name")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "The {0} is required.")]
    [StringLength(100, ErrorMessage = "The {0} cannot be longer than {1} characters.")]
    [Display(Name = "Address")]
    public required string Address { get; set; }

    [Required(ErrorMessage = "The {0} is required.")]
    [Range(1, 5000, ErrorMessage = "The {0} must be between {1} and {2}.")]
    [Display(Name = "Capacity (Beds)")]
    public int Capacity { get; set; }

    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;
}