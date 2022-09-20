using WebApi.Common.Constants;

namespace WebApplication1.Response;

public class PaginationBaseResponse<T>
{
    public int CurrentPage { get; set; } = Base.Pagination.DefaultCurrentPage;
    public int PageSize { get; set; } = Base.Pagination.DefaultSize;

    public int TotalPages { get; set; } = Base.Pagination.DefaultTotalPages;

    public int TotalItems { get; set; } = Base.Pagination.DefaultTotalItems;

    public string? OrderBy { get; set; } = Base.Pagination.DefaultOrderBy;

    public bool OrderByDesc { get; set; } = Base.Pagination.DefaultOrderByDesc;
    public List<T> Result { get; set; } = new List<T>();

}