using System;
using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AccommodationManagement;

public class ApplicationStatusHistory
{
    public int ApplicationStatusHistoryId { get; set; }

    [Display(Name = "ID da Candidatura")]
    public int AccommodationApplicationId { get; set; } 

    [Display(Name = "Data de Alteração")]
    public DateTime ChangedAt { get; set; }

    [Display(Name = "Alterado Por")]
    public string ChangedBy { get; set; } = string.Empty;

}