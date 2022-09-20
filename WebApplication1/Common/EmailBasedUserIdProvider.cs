
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace WebApplication1;

public class EmailBasedUserIdProvider : IUserIdProvider
{
    public virtual string GetUserId(HubConnectionContext connection)
    {
        var b = connection.User?.FindFirst(ClaimTypes.Email)?.Value!;
        return  b;
    }
}