namespace PetCore.Application.DTOs;

public sealed class PageQuery
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
    public string? SortBy { get; init; }
    public bool Descending { get; init; }
    public IReadOnlyDictionary<string, string> Filters { get; init; } = new Dictionary<string, string>();
}

public interface IPagedData
{
    object ItemsObject { get; }
    int Page { get; }
    int PageSize { get; }
    int TotalItems { get; }
    int TotalPages { get; }
}

public sealed record PagedData<T>(IReadOnlyCollection<T> Items, int Page, int PageSize, int TotalItems, int TotalPages) : IPagedData
{
    public object ItemsObject => Items;
}
