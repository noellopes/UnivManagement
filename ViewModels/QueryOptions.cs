using Microsoft.EntityFrameworkCore;

namespace UnivManagement.ViewModels {
    public interface IPagination {
        public int CurrentPage { get; }
        public int PageCount { get; }
        public int FirstPageToShow { get; }
        public int LastPageToShow { get; }
    }

    public class QueryOptions<T> : IPagination {
        private const int NUMBER_PAGES_AROUND_SELECTED = 5;

        private QueryOptions(IEnumerable<T> items, int currentPage, int itemsPerPage, int totalItems, int pageCount) {
            Items = items;
            TotalItems = totalItems;
            CurrentPage = currentPage;
            ItemsPerPage = itemsPerPage;
            PageCount = pageCount;
        }

        public static async Task<QueryOptions<T>> CreateAsync(
            IQueryable<T> queryData,
            int currentPage,
            int itemsPerPage = 10)
        {
            int totalItems = await queryData.CountAsync();

            int pageCount = Math.Max(1, (int)Math.Ceiling((double)totalItems / itemsPerPage));

            currentPage = Math.Clamp(currentPage, 1, pageCount);

            var items = await queryData
                .Skip(itemsPerPage * (currentPage - 1))
                .Take(itemsPerPage)
                .ToListAsync();

            return new QueryOptions<T>(
                items,
                currentPage,
                itemsPerPage,
                totalItems,
                pageCount
            );
        }

        public IEnumerable<T> Items { get; private set; }

        public int TotalItems { get; }
        public int ItemsPerPage { get; }
        public int CurrentPage { get; }
        public int PageCount { get; }

        public int FirstPageToShow => Math.Max(1, CurrentPage - NUMBER_PAGES_AROUND_SELECTED);
        public int LastPageToShow => Math.Min(PageCount, CurrentPage + NUMBER_PAGES_AROUND_SELECTED);
    }
}
