using PetCore.Application.DTOs;
using PetCore.Authentication;

namespace PetCore.Extensions;

public static class PaginationExtensions
{
    private static readonly HashSet<string> Reserved = ["page", "pageSize", "sortBy", "descending"];

    public static bool IsPaginationRequested(this IQueryCollection query) =>
        Reserved.Any(key => query.ContainsKey(key));
    public static PageQuery ToPageQuery(this PaginationOptions options, IQueryCollection query) => new()
    {
        Page = Math.Max(1, options.Page),
        PageSize = Math.Clamp(options.PageSize, 1, 100),
        SortBy = options.SortBy,
        Descending = options.Descending,
        Filters = query.Where(item => !Reserved.Contains(item.Key))
            .ToDictionary(item => item.Key, item => item.Value.ToString(), StringComparer.OrdinalIgnoreCase)
    };
}
