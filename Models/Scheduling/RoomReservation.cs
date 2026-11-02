
namespace UnivManagement.Models.Scheduling
{
    public class RoomReservation
    {
        public int RoomReservationId { get; set; }

        public int RoomId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string Purpose { get; set; } = "";
    }
}

