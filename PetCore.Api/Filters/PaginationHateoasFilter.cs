using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.WebUtilities;
using PetCore.Application.DTOs;
using PetCore.Authentication;
using PetCore.Extensions;

namespace PetCore.Filters;

public sealed class PaginationHateoasFilter : IAsyncResultFilter
{
    private static readonly HashSet<string> Reserved = ["page", "pageSize", "sortBy", "descending"];

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.HttpContext.Request.Method == "GET" && context.Result is ObjectResult result && result.Value is not null && result.Value is not string)
        {
            var request = context.HttpContext.Request;
            if (result.Value is IPagedData paged)
            {
                var links = new List<HateoasLink>
                {
                    new(request.Path + request.QueryString, "self"),
                    new(request.Path, "collection")
                };
                if (paged.Page > 1) links.Add(new(BuildPageLink(request.Path, request.Query, paged.Page - 1, paged.PageSize), "previous"));
                if (paged.Page < paged.TotalPages) links.Add(new(BuildPageLink(request.Path, request.Query, paged.Page + 1, paged.PageSize), "next"));
                var items = ((IEnumerable)paged.ItemsObject).Cast<object>().ToList();
                result.Value = new PagedResponse<object>(items, paged.Page, paged.PageSize, paged.TotalItems, paged.TotalPages, links);
            }
            else if (result.Value is IEnumerable items)
            {
                var values = items.Cast<object>().ToList();
                foreach (var filter in request.Query.Where(x => !Reserved.Contains(x.Key)))
                {
                    var property = values.FirstOrDefault()?.GetType().GetProperties().FirstOrDefault(p => p.Name.Equals(filter.Key, StringComparison.OrdinalIgnoreCase));
                    if (property is not null)
                        values = values.Where(item => property.GetValue(item)?.ToString()?.Contains(filter.Value.ToString(), StringComparison.OrdinalIgnoreCase) == true).ToList();
                }

                var sortBy = request.Query["sortBy"].FirstOrDefault();
                var sort = values.FirstOrDefault()?.GetType().GetProperties().FirstOrDefault(p => p.Name.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
                if (sort is not null)
                    values = (request.Query["descending"] == "true" ? values.OrderByDescending(x => sort.GetValue(x)) : values.OrderBy(x => sort.GetValue(x))).ToList();

                var page = Math.Max(1, int.TryParse(request.Query["page"], out var requestedPage) ? requestedPage : 1);
                var size = Math.Clamp(int.TryParse(request.Query["pageSize"], out var requestedSize) ? requestedSize : 20, 1, 100);
                var total = values.Count;
                var pages = (int)Math.Ceiling(total / (double)size);
                var links = new List<HateoasLink> { new(request.Path + request.QueryString, "self"), new(request.Path, "collection") };
                if (page > 1) links.Add(new(BuildPageLink(request.Path, request.Query, page - 1, size), "previous"));
                if (page < pages) links.Add(new(BuildPageLink(request.Path, request.Query, page + 1, size), "next"));
                result.Value = new PagedResponse<object>(values.Skip((page - 1) * size).Take(size).ToList(), page, size, total, pages, links);
            }
            else if (result.Value.GetType().GetProperty("Id")?.GetValue(result.Value) is { } id)
            {
                var collection = request.Path.Value!.Split('/')[..^1].Aggregate((a, b) => a + "/" + b);
                result.Value = new ResourceResponse<object>(result.Value,
                [new(request.Path, "self"), new(collection, "collection"), new(request.Path + "/patch", "update", "PUT"), new(request.Path, "delete", "DELETE")]);
            }
        }
        await next();
    }

    private static string BuildPageLink(PathString path, IQueryCollection original, int page, int size)
    {
        var query = original.ToDictionary(item => item.Key, item => (string?)item.Value.ToString(), StringComparer.OrdinalIgnoreCase);
        query["page"] = page.ToString();
        query["pageSize"] = size.ToString();
        return QueryHelpers.AddQueryString(path, query);
    }
}
