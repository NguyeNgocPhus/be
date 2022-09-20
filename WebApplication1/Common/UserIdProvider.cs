

using Microsoft.AspNetCore.SignalR;

namespace WebApplication1.Common;

public class NameUserIdProvider : IUserIdProvider
{
    public string GetUserId(HubConnectionContext connection)
    {
        var a = connection.User.Identity?.Name;
        return a;
    }
}