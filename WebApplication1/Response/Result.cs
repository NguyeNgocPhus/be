namespace WebApplication1.Response;

public class Result<TDto> where TDto : class
{
    
    public TDto Data { get; set; }
    public bool Success { get; set; }
    public bool Forbidden { get;  set; }

    public IEnumerable<ErrorItem> Errors { get;  set; }
    public Result(bool success, IEnumerable<ErrorItem> errors, TDto data, bool forbidden)
    {
        Success = success;
        Forbidden = forbidden;
        Data = data;
        Errors = errors;
    }

    public static Result<TDto> Succeed(TDto data = null)
    {
        return new Result<TDto>(true, new ErrorItem[] { }, data, false);
    }
    public static Result<TDto> Fail(IEnumerable<ErrorItem> errors, TDto data = null)
    {
        return new Result<TDto>(false, errors, data, false);
    }
}