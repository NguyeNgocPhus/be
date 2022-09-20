using Identity.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApplication1.Database;
using WebApplication1.Entities;
using WebApplication1.Input;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers;

[Authorize]
[ApiController]
public class NotificationController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly JwtConfiguration _jwtOption;
    private readonly ICurrentAccountServices _currentAccount;
    
    public NotificationController(ApplicationDbContext context,  IOptions<JwtConfiguration> jwtOption, ICurrentAccountServices currentAccount)
    {
        _context = context;
        _jwtOption = jwtOption.Value;
        _currentAccount = currentAccount;
    }

    [HttpPost]
    [Route("/notification")]
    public async Task<NotificationEntity> CreateNotification(CreateNotificationInput input)
    {
        var notification = new NotificationEntity()
        {
            Id = Guid.NewGuid(),
            Content = input.Content,
            NotificationType = input.NotificationType,
            Opened = false,
            RoomId = input.RoomId,
            UserFrom = _currentAccount.Id,
            UserTo = input.UserTo
        };
        await _context.AddAsync(notification);
        await _context.SaveChangesAsync();
        return notification;
    }    
    
}