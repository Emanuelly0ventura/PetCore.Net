using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.WebUtilities;
using PetCore.Application.DTOs;
using PetCore.Authentication;

namespace PetCore.Filters;

public sealed class PaginationHateoasFilter : IAsyncResultFilter
{
    private static readonly string[] PaginationKeys = ["page", "pageSize", "sortBy", "descending"];

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.Result is not ObjectResult result || result.Value is null || result.Value is string)
        {
            await next();
            return;
        }

        var request = context.HttpContext.Request;
        var statusCode = result.StatusCode ?? context.HttpContext.Response.StatusCode;
        if (statusCode is >= 400 or < 200 || request.Path.Value?.EndsWith("/login", StringComparison.OrdinalIgnoreCase) == true)
        {
            await next();
            return;
        }

        if (request.Method == "GET")
        {
            if (result.Value is IPagedData paged)
            {
                var links = BuildCollectionLinks(request.Path, request.Query, paged.Page, paged.PageSize, paged.TotalPages);
                SetLinkHeader(context, links);
                var usesPagedContract = PaginationKeys.Any(key => request.Query.ContainsKey(key));
                result.Value = usesPagedContract
                    ? new PagedResponse<object>(((IEnumerable)paged.ItemsObject).Cast<object>().ToList(), paged.Page, paged.PageSize, paged.TotalItems, paged.TotalPages, links)
                    : paged.ItemsObject;
            }
            else if (result.Value is IEnumerable items)
            {
                var values = items.Cast<object>().ToList();
                foreach (var filter in request.Query.Where(item => !PaginationKeys.Contains(item.Key, StringComparer.OrdinalIgnoreCase)))
                {
                    var property = values.FirstOrDefault()?.GetType().GetProperties()
                        .FirstOrDefault(candidate => candidate.Name.Equals(filter.Key, StringComparison.OrdinalIgnoreCase));
                    if (property is not null)
                        values = values.Where(item => property.GetValue(item)?.ToString()?.Contains(filter.Value.ToString(), StringComparison.OrdinalIgnoreCase) == true).ToList();
                }

                var sortBy = request.Query["sortBy"].FirstOrDefault();
                var sort = values.FirstOrDefault()?.GetType().GetProperties()
                    .FirstOrDefault(property => property.Name.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
                if (sort is not null)
                    values = (request.Query["descending"] == "true"
                        ? values.OrderByDescending(item => sort.GetValue(item))
                        : values.OrderBy(item => sort.GetValue(item))).ToList();

                var page = Math.Max(1, int.TryParse(request.Query["page"], out var requestedPage) ? requestedPage : 1);
                var size = Math.Clamp(int.TryParse(request.Query["pageSize"], out var requestedSize) ? requestedSize : 20, 1, 100);
                var pages = (int)Math.Ceiling(values.Count / (double)size);
                var links = BuildCollectionLinks(request.Path, request.Query, page, size, pages);
                SetLinkHeader(context, links);
                result.Value = PaginationKeys.Any(key => request.Query.ContainsKey(key))
                    ? new PagedResponse<object>(values.Skip((page - 1) * size).Take(size).ToList(), page, size, values.Count, pages, links)
                    : values;
            }
            else if (result.Value is not IEnumerable && result.Value.GetType().GetProperty("Id")?.GetValue(result.Value) is not null)
            {
                var links = new List<HateoasLink>
                {
                    new(request.Path + request.QueryString, "self"),
                    new(GetCollectionPath(request.Path), "collection"),
                    new(request.Path + "/patch", "update", "PUT"),
                    new(request.Path, "patch", "PATCH"),
                    new(request.Path, "delete", "DELETE")
                };
                SetLinkHeader(context, links);
            }
        }

        if (ShouldEnvelope(context))
            result.Value = Wrap(result.Value);

        await next();
    }

    private static bool ShouldEnvelope(ResultExecutingContext context)
    {
        if (context.ActionDescriptor is not ControllerActionDescriptor action)
            return false;

        return action.ControllerName switch
        {
            "Pet" => true,
            "Tutor" => !action.ActionName.Equals("Login", StringComparison.OrdinalIgnoreCase),
            "Medico" => !action.ActionName.Equals("Login", StringComparison.OrdinalIgnoreCase)
                && (!action.ActionName.Equals("FetchAll", StringComparison.OrdinalIgnoreCase)
                    || PetCore.Authorization.ResourceAccess.IsVeterinarian(context.HttpContext.User)),
            _ => false
        };
    }

    private static object Wrap(object value)
    {
        if (value is PagedResponse<object> page)
            return new PagedApiResponse<object>(page.Items, page.Page, page.PageSize, page.TotalCount, page.TotalPages, page.Links);

        if (value is IEnumerable items)
            return new ApiResponseEnvelope<IReadOnlyCollection<object>>(items.Cast<object>().ToList());

        return new ApiResponseEnvelope<object>(value);
    }

    private static List<HateoasLink> BuildCollectionLinks(PathString path, IQueryCollection original, int page, int size, int pages)
    {
        var links = new List<HateoasLink> { new(BuildPageLink(path, original, page, size), "self"), new(path, "collection") };
        if (pages > 0) links.Add(new(BuildPageLink(path, original, 1, size), "first"));
        if (pages > 0) links.Add(new(BuildPageLink(path, original, pages, size), "last"));
        if (page > 1) links.Add(new(BuildPageLink(path, original, page - 1, size), "previous"));
        if (page < pages) links.Add(new(BuildPageLink(path, original, page + 1, size), "next"));
        return links;
    }

    private static void SetLinkHeader(ResultExecutingContext context, IEnumerable<HateoasLink> links)
    {
        context.HttpContext.Response.Headers.Link = string.Join(", ", links.Select(link => $"<{link.Href}>; rel=\"{link.Rel}\""));
    }

    private static string GetCollectionPath(PathString path)
    {
        var value = path.Value ?? "/";
        var slash = value.LastIndexOf('/');
        return slash > 0 ? value[..slash] : value;
    }

    private static string BuildPageLink(PathString path, IQueryCollection original, int page, int size)
    {
        var query = original.ToDictionary(item => item.Key, item => (string?)item.Value.ToString(), StringComparer.OrdinalIgnoreCase);
        query["page"] = page.ToString();
        query["pageSize"] = size.ToString();
        return QueryHelpers.AddQueryString(path, query);
    }
}
