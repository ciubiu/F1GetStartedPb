using Microsoft.EntityFrameworkCore;

namespace F1LapsGame.Models;

public class DataViewPage<T> : List<T>
{
    public int CurrentPage { get; private set; }
    public int TotalPages { get; private set; }

    public DataViewPage(List<T> items, int count, int pageNumber, int pageSize)
    {
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);

        AddRange(items);
    }

    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;

    public static async Task<DataViewPage<T>> CreateAsync(IQueryable<T> query, int pageNumber, int pageSize)
    {
        var count = await query.CountAsync();
        // TODO; add OrderBy to prevent unpredictable results
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new DataViewPage<T>(items, count, pageNumber, pageSize);
    }
}