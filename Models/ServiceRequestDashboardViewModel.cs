namespace UnivManagement.Models {
    // Data shown on the main page of the Requests and Administrative Services module
    public class ServiceRequestDashboardViewModel {
        public int TotalRequests { get; set; }

        public int OpenRequests { get; set; }

        public int UnassignedRequests { get; set; }

        public int ClosedRequests { get; set; }

        // Every status, in its order, with the number of requests that have it
        public List<KeyValuePair<ServiceRequestStatus, int>> RequestsByStatus { get; set; } = new();

        // Every request type with its number of requests
        public List<KeyValuePair<ServiceRequestType, int>> RequestsByType { get; set; } = new();

        public List<ServiceRequest> LatestRequests { get; set; } = new();
    }
}
