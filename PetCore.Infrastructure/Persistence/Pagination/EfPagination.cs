using System.Globalization;
using System.Linq.Expressions;
using PetCore.Application.DTOs;

namespace PetCore.Infrastructure.Persistence.Pagination;

internal static class EfPagination
{
    public static PagedData<T> FetchPage<T>(IQueryable<T> source, PageQuery request)
    {
        var page = Math.Max(1, request.Page);
        var size = Math.Clamp(request.PageSize, 1, 100);
        var query = source;
        foreach (var (key, value) in request.Filters)
        {
            var path = ResolvePath(typeof(T), key);
            var filterValue = value;
            if (key.Equals("Genero", StringComparison.OrdinalIgnoreCase))
                filterValue = value.Equals("Feminino", StringComparison.OrdinalIgnoreCase) ? "F" : value.Equals("Masculino", StringComparison.OrdinalIgnoreCase) ? "M" : value;
            if (path is null || !TryBuildFilter(typeof(T), path, filterValue, out var predicate)) continue;
            query = query.Where((Expression<Func<T, bool>>)predicate);
        }
        var sortPath = request.SortBy is null ? null : ResolvePath(typeof(T), request.SortBy);
        query = ApplySort(query, sortPath ?? ResolvePath(typeof(T), "Id") ?? "Id", request.Descending);
        var total = query.Count();
        var offset = (int)Math.Min((long)(page - 1) * size, int.MaxValue);
        var items = query.Skip(offset).Take(size).ToList();
        return new PagedData<T>(items, page, size, total, (int)Math.Ceiling(total / (double)size));
    }

    private static string? ResolvePath(Type type, string name)
    {
        var aliases = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        if (type.Name == "Clinica") { aliases["Cep"] = "Endereco.Cep"; aliases["Complemento"] = "Endereco.Complemento"; }
        if (type.Name is "Tutor" or "Medico") { aliases["Nascimento"] = "DataNascimento"; aliases["Genero"] = "Sexo"; aliases["fotoUrl"] = "UrlImg"; }
        if (type.Name == "Medico") aliases["Especializacao"] = "Especialidade";
        if (type.Name == "Exame") aliases["tipoExame"] = "Tipo";
        if (aliases.TryGetValue(name, out var alias)) name = alias;
        var path = name.Split('.');
        var current = type;
        var resolved = new List<string>(path.Length);
        foreach (var part in path)
        {
            var property = current.GetProperties().FirstOrDefault(item => item.Name.Equals(part, StringComparison.OrdinalIgnoreCase));
            if (property is null) return null;
            resolved.Add(property.Name);
            current = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
        }
        return string.Join('.', resolved);
    }

    private static bool TryBuildFilter(Type type, string path, string value, out LambdaExpression predicate)
    {
        var parameter = Expression.Parameter(type, "item");
        Expression member = parameter;
        foreach (var part in path.Split('.')) member = Expression.Property(member, part);
        var targetType = Nullable.GetUnderlyingType(member.Type) ?? member.Type;
        Expression body;
        if (targetType == typeof(string))
        {
            var notNull = Expression.NotEqual(member, Expression.Constant(null, member.Type));
            var normalized = Expression.Call(member, typeof(string).GetMethod(nameof(string.ToLower), Type.EmptyTypes)!);
            var contains = Expression.Call(normalized, typeof(string).GetMethod(nameof(string.Contains), [typeof(string)])!, Expression.Constant(value.ToLowerInvariant()));
            body = Expression.AndAlso(notNull, contains);
        }
        else
        {
            if (!TryConvert(value, targetType, out var converted)) { predicate = null!; return false; }
            var comparison = Expression.Equal(member.Type == targetType ? member : Expression.Convert(member, targetType), Expression.Constant(converted, targetType));
            body = Nullable.GetUnderlyingType(member.Type) is null ? comparison : Expression.AndAlso(Expression.NotEqual(member, Expression.Constant(null, member.Type)), comparison);
        }
        predicate = Expression.Lambda(body, parameter);
        return true;
    }

    private static IQueryable<T> ApplySort<T>(IQueryable<T> source, string path, bool descending)
    {
        var parameter = Expression.Parameter(typeof(T), "item");
        Expression property = parameter;
        foreach (var part in path.Split('.')) property = Expression.Property(property, part);
        var selector = Expression.Lambda(property, parameter);
        var call = Expression.Call(typeof(Queryable), descending ? "OrderByDescending" : "OrderBy", [typeof(T), property.Type], source.Expression, Expression.Quote(selector));
        return source.Provider.CreateQuery<T>(call);
    }

    private static bool TryConvert(string value, Type type, out object converted)
    {
        if (type == typeof(Guid) && Guid.TryParse(value, out var guid)) { converted = guid; return true; }
        if (type == typeof(DateOnly) && DateOnly.TryParse(value, CultureInfo.InvariantCulture, out var date)) { converted = date; return true; }
        if (type.IsEnum && Enum.TryParse(type, value, true, out var enumeration)) { converted = enumeration!; return true; }
        try { converted = Convert.ChangeType(value, type, CultureInfo.InvariantCulture)!; return true; }
        catch (Exception exception) when (exception is FormatException or InvalidCastException or OverflowException) { converted = null!; return false; }
    }
}
