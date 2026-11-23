using Microsoft.EntityFrameworkCore;

namespace UnivManagement.ViewModels
{
    public class QueryOptions<T>
    {
        private QueryOptions(
            IEnumerable<T> items,
            int currentPage,
            int itemsPerPage,
            int totalItems)
        {
            Items = items;
            TotalItems = totalItems;
            CurrentPage = currentPage;
            ItemsPerPage = itemsPerPage;
        }

        public static async Task<QueryOptions<T>> CreateAsync(
            IQueryable<T> queryData,
            int currentPage,
            int itemsPerPage = 10)
        {
            int totalItems = await queryData.CountAsync();

            if (currentPage < 1)
            {
                currentPage = 1;
            }

            int totalPages =
                (totalItems + itemsPerPage - 1) / itemsPerPage;

            if (totalPages > 0 && currentPage > totalPages)
            {
                currentPage = totalPages;
            }

            var items = await queryData
                .Skip(itemsPerPage * (currentPage - 1))
                .Take(itemsPerPage)
                .ToListAsync();

            return new QueryOptions<T>(
                items,
                currentPage,
                itemsPerPage,
                totalItems);
        }

        public IEnumerable<T> Items { get; private set; }

        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
    }
}