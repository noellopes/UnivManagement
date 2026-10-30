namespace UnivManagement.Models {
    // Data shown on the main page of the Requests and Administrative Services module
    public class ServiceRequestDashboardViewModel {
        public int TotalRequests { get; set; }

        public int OpenRequests { get; set; }

        public int UnassignedRequests { get; set; }

        public int ClosedRequests { get; set; }

        public int OverdueRequests { get; set; }

        // Average time (in days) between the creation and the closing of a request
        public double? AverageDaysToClose { get; set; }

        // Every status, in its order, with the number of requests that have it
        public List<KeyValuePair<ServiceRequestStatus, int>> RequestsByStatus { get; set; } = new();

        // Every request type with its number of requests
        public List<KeyValuePair<ServiceRequestType, int>> RequestsByType { get; set; } = new();

        // Every priority, the most urgent first, with its number of open requests
        public List<KeyValuePair<ServiceRequestPriority, int>> OpenRequestsByPriority { get; set; } = new();

        public List<ServiceRequest> LatestRequests { get; set; } = new();

        // Open requests whose due date has passed, the oldest first
        public List<ServiceRequest> OverdueList { get; set; } = new();

        public int PercentOfOpen(int count) {
            return OpenRequests == 0 ? 0 : count * 100 / OpenRequests;
        }
    }
}
