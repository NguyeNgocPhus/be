using WebApplication1.Entities;
using WebApplication1.Input;
using WebApplication1.Response;

namespace WebApplication1.Services.Interfaces;

public interface IMessageService
{
    public Task<Result<PaginationBaseResponse<MessageEntity>>> GetMessage(Guid chatId,ViewListMessageInput input, CancellationToken cancellationToken);
}