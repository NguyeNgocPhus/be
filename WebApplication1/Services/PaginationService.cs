using WebApi.Common.Constants;
using WebApplication1.Response;
using WebApplication1.Services.Interfaces;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Services;

public class PaginationService : IPaginationService
{
    public async Task<PaginationBaseResponse<T>> PaginateAsync<T>(IQueryable<T> source, int page, string? orderBy, bool orderByDesc, int pageSize, CancellationToken cancellationToken = default) where T : class
    {
        if (page == 0) page = Base.Pagination.DefaultPage;
        if (pageSize == 0) pageSize = Base.Pagination.DefaultSize;
        var paginationResponse = new PaginationBaseResponse<T>()
        {
            PageSize = pageSize,
            CurrentPage = page,
            TotalPages = (int)Math.Ceiling((double)source.Count()/pageSize),
            OrderBy = orderBy,
            OrderByDesc = orderByDesc,
            TotalItems = source.Count()
            
        };
        var skip = (page - 1) * pageSize;
        var sortRequired = orderBy != null;
        var order = orderByDesc ? "DESC" : "ASC";
        orderBy = orderBy?.ToLower();

        if (sortRequired)
        {
            paginationResponse.Result = await source
                .OrderBy($"{orderBy} {order}")
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }
        else
        {
            paginationResponse.Result = await source
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        return paginationResponse;

    }
}