using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using PetCore.Application.DTOs;

namespace PetCore.Authorization;

/// <summary>Obtém a identidade autenticada e aplica escopo de propriedade a DTOs de recursos.</summary>
public static class ResourceAccess
{
    private static readonly string[] VeterinarianIdProperties =
        ["IdMedico", "IdMedicoResponsavel", "VeterinarioId"];

    public static Guid? GetUserId(ClaimsPrincipal principal)
    {
        var value = principal.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? principal.FindFirstValue(JwtRegisteredClaimNames.Sub)
            ?? principal.FindFirstValue("sub");

        return Guid.TryParse(value, out var id) && id != Guid.Empty ? id : null;
    }

    public static bool IsTutor(ClaimsPrincipal principal) => principal.IsInRole("tutor");

    public static bool IsVeterinarian(ClaimsPrincipal principal) =>
        principal.IsInRole("medico") || principal.IsInRole("veterinario");

    public static bool IsOwnProfile(ClaimsPrincipal principal, Guid profileId) =>
        GetUserId(principal) == profileId;

    public static bool CanAccessRecord(ClaimsPrincipal principal, object record)
    {
        var userId = GetUserId(principal);
        if (!userId.HasValue)
            return false;

        if (IsTutor(principal))
            return MatchesGuidProperty(record, "TutorId", userId.Value);

        return IsVeterinarian(principal)
            && VeterinarianIdProperties.Any(property => MatchesGuidProperty(record, property, userId.Value));
    }

    public static IReadOnlyCollection<T> FilterRecords<T>(ClaimsPrincipal principal, IEnumerable<T> records) =>
        records.Where(record => record is not null && CanAccessRecord(principal, record)).ToList();

    public static PageQuery ScopeRecordPageQuery(
        ClaimsPrincipal principal,
        PageQuery query,
        string tutorProperty,
        string veterinarianProperty)
    {
        var filters = new Dictionary<string, string>(query.Filters, StringComparer.OrdinalIgnoreCase);
        var ownerProperty = IsTutor(principal) ? tutorProperty : veterinarianProperty;
        filters[ownerProperty] = GetUserId(principal)?.ToString() ?? Guid.Empty.ToString();

        return new PageQuery
        {
            Page = query.Page,
            PageSize = query.PageSize,
            SortBy = query.SortBy,
            Descending = query.Descending,
            Filters = filters
        };
    }

    private static bool MatchesGuidProperty(object instance, string propertyName, Guid expected)
    {
        var property = instance.GetType().GetProperty(propertyName);
        if (property?.GetValue(instance) is not Guid value)
            return false;

        return value == expected;
    }
}
