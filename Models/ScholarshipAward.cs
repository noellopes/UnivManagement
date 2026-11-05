namespace UnivManagement.Models
{
    public class ScholarshipAward
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public decimal Amount { get; set; }
        public DateTime AwardDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Active";
    }
}