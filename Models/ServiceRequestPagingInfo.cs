namespace UnivManagement.Models
{
    // Page information for the list of service requests
    public class ServiceRequestPagingInfo
    {
        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int TotalItems { get; set; }

        public int TotalPages => Math.Max(1, (int)Math.Ceiling((double)TotalItems / PageSize));

        public bool HasPreviousPage => CurrentPage > 1;

        public bool HasNextPage => CurrentPage < TotalPages;

        // Position of the first and last item shown, for texts like "Showing 11 to 20 of 45"
        public int FirstItem => TotalItems == 0 ? 0 : (CurrentPage - 1) * PageSize + 1;

        public int LastItem => Math.Min(CurrentPage * PageSize, TotalItems);

        // Pages that get a link: the current one and up to two at each side
        public IEnumerable<int> VisiblePages()
        {
            int first = Math.Max(1, CurrentPage - 2);
            int last = Math.Min(TotalPages, CurrentPage + 2);
            return Enumerable.Range(first, last - first + 1);
        }
    }
}
