using WebApplication1.Response;

namespace WebApplication1.Services.Interfaces;

public interface IPaginationService
{
    Task<PaginationBaseResponse<T>> PaginateAsync<T>(IQueryable<T> source, int page, string? orderBy, bool orderByDesc, int pageSize, CancellationToken cancellationToken = default) where T : class;
}