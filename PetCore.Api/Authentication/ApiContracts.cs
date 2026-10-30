using System.ComponentModel.DataAnnotations;
namespace PetCore.Authentication;
public sealed record LoginRequest(string Email, string Password, string Role);
public sealed record HateoasLink(string Href, string Rel, string Method = "GET");
public sealed record PagedResponse<T>(IReadOnlyCollection<T> Items, int Page, int PageSize, int TotalItems, int TotalPages, IReadOnlyCollection<HateoasLink> Links);
public sealed record ResourceResponse<T>(T Data, IReadOnlyCollection<HateoasLink> Links);
public sealed class PaginationOptions
{
    /// <example>1</example>
    [Range(1, int.MaxValue)] public int Page { get; set; } = 1;
    /// <example>20</example>
    [Range(1, 100)] public int PageSize { get; set; } = 20;
    /// <example>Nome</example>
    public string? SortBy { get; set; }
    public bool Descending { get; set; }
}
