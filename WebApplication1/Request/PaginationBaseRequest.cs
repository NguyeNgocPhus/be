using WebApi.Common.Constants;

namespace WebApplication1.Request;

public class PaginationBaseRequest
{
    public string? Keyword { get; set; }
    public string? OrderBy { get; set; }
    public bool OrderByDesc { get; set; }
    public int Page { get; set; } = Base.Pagination.DefaultPage;
    public int Size { get; set; } =  Base.Pagination.DefaultSize;
}