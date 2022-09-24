using System.Collections.Generic;

namespace WorkTimer.Domain.Models;
public class PagedResult<T> where T : class
{
    public PagedResult(IEnumerable<T> items, int count)
    {
        Items = items ?? new List<T>();
        Count = count;
    }

    public IEnumerable<T> Items { get; }
    public int Count { get; }
}