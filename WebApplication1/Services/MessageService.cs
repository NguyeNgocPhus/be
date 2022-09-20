using Identity.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApplication1.Database;
using WebApplication1.Entities;
using WebApplication1.Input;
using WebApplication1.Response;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services;

public class MessageService : IMessageService
{
    private readonly ApplicationDbContext _context;
    private readonly JwtConfiguration _jwtOption;
    private readonly ICurrentAccountServices _currentAccount;
    private readonly IPaginationService _paginationService;
    private readonly ILogger _logger;

    public MessageService(ApplicationDbContext context,  IOptions<JwtConfiguration> jwtOption, ICurrentAccountServices currentAccount, IPaginationService paginationService, ILogger<MessageService> logger)
    {
        _context = context;
        _jwtOption = jwtOption.Value;
        _currentAccount = currentAccount;
        _paginationService = paginationService;
        _logger = logger;
    }


    public async Task<Result<PaginationBaseResponse<MessageEntity>>> GetMessage(Guid chatId , ViewListMessageInput input,CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        var queryable =  _context.Messages.Where(x => x.ChatRoom == chatId).OrderBy(x=>x.CreateTime).AsSplitQuery();
        var result = await _paginationService.PaginateAsync(queryable, input.Page, input.OrderBy, input.OrderByDesc, input.Size, cancellationToken);
        if (result.Result.Count() == 0)
        {
            
        }
        _logger.LogInformation("hello",DateTime.UtcNow.ToLongTimeString());
        
        return Result<PaginationBaseResponse<MessageEntity>>.Succeed(result);

    }
}