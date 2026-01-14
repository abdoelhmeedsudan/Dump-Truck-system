using Microsoft.EntityFrameworkCore;

namespace DumpTruckManagementSystem.Shared.Wrappers
{
    public class PagedList<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
        public List<T> Items { get; set; } = new();

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize <= 0 ? 10 : pageSize;
            CurrentPage = pageNumber <= 0 ? 1 : pageNumber;

            TotalPages = (int)Math.Ceiling(count / (double)PageSize);
            Items = items ?? new List<T>();
        }

        // ✅ New overload with CancellationToken (recommended)
        public static async Task<PagedList<T>> ToPagedListAsync(
            IQueryable<T> query,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 10 : pageSize;

            var count = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }

        // ✅ Keep old signature for backward compatibility
        public static Task<PagedList<T>> ToPagedListAsync(
            IQueryable<T> query,
            int pageNumber,
            int pageSize)
        {
            return ToPagedListAsync(query, pageNumber, pageSize, CancellationToken.None);
        }
    }
}
