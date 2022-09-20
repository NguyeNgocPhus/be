
using System.Runtime.CompilerServices;
using System.Threading.Channels;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WebApplication1.Entities;
using WebApplication1.Services.Interfaces;
using Hub = Microsoft.AspNetCore.SignalR.Hub;


namespace WebApplication1.Hubs;

public class Message {
    public string content { get; set; }
    public string room { get; set; }
    public List<object> liker { get; set; }
    public string avatar { get; set; }
    public string senderId { get; set; }
    public string sender { get; set; }
    public string id { get; set; }
}
public class ChatHubs : Hub
{
    public async Task PostMessage(object message)
    {
        await Clients.Others.SendAsync("PostMessage",message);
    }

    public async Task LikeMessage(Message message)
    {
        await Clients.Group(message.room).SendAsync("LikeMessage",message);
    }
    // gui message ve client
    public async Task SendMessageByClient(Message message) 
    {
        await Clients.Group(message.room).SendAsync("SendMessageByServer",message);
        await Clients.Group(message.room).SendAsync($"MessageTo-{message.room}",message);
    }
    // join vao cac chat room cua nguoi dung
    public async Task SetUpRoom(List<ChatRoomEntity> listRoom)
    {
        foreach (var room in listRoom)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, room.Id.ToString());
        }
    }
    // joi vao chat room vs group la cua nguoi dung
    public async Task SetUp(UserEntity profile)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, profile.Id.ToString());
    }
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
    
}