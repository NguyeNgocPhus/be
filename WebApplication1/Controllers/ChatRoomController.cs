using Identity.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApplication1.Common.Attributes;
using WebApplication1.Database;
using WebApplication1.Entities;
using WebApplication1.Input;
using WebApplication1.Services.Interfaces;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers;

[Authorize]
[ApiController]
public class ChatRoomController: ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly JwtConfiguration _jwtOption;
    private readonly ICurrentAccountServices _currentAccount;

    public ChatRoomController(ApplicationDbContext context,  IOptions<JwtConfiguration> jwtOption, ICurrentAccountServices currentAccount)
    {
        _context = context;
        _jwtOption = jwtOption.Value;
        _currentAccount = currentAccount;
    }
    
    
    
    [HttpPost]
    [Route("/chatroom")]
    public async Task<ChatRoomEntity> CreateChatRoom(ChatRoomInput input)
    {
        var users = _context.Users.Where(x => input.Users.Contains(x.Id)).ToList();
        var myprofile = await _context.Users.FirstOrDefaultAsync(x => x.Id == _currentAccount.Id);
        var a = users.FirstOrDefault(x => x.Id == myprofile.Id);
        if (a == null) 
        {
            users.Add(myprofile);
        }
       
        var chat = new ChatRoomEntity()
        {
            Id = Guid.NewGuid(),
            Name = input.Name,
            LatestMessage = String.Empty,
            Users = users
        };
        await _context.AddAsync(chat);
        await _context.SaveChangesAsync();
        return chat;
    }

    [HttpGet]
    [Route("/chatroom/{chatId}")]
    public async Task<ChatRoomEntity> ViewChatRoom(Guid chatId)
    {
        var chatRoom = await _context.Chats.FirstOrDefaultAsync(x => x.Id == chatId);
        return chatRoom;
    }
    [HttpGet]
    [Route("/chatroom")]
    public async Task<IQueryable<ChatRoomViewModel>> ViewChatRooms()
    {
        var b = _currentAccount.Id;
        var chatRoom = await Task.FromResult(_context.ChatViewModels.Where(x=>x.UserId == _currentAccount.Id));
        return chatRoom;
    }
    
    [HttpPut]
    [Route("/chatroom/{chatId}")]
    public async Task<ChatRoomEntity> UpdateChatRoom(UpdateChatRoomInput input, Guid chatId)
    {
        var chatRoom = await _context.Chats.FirstOrDefaultAsync(x => x.Id == chatId);
        var users = new List<UserEntity>();
        if (input.Users.Count > 0)
        {
            foreach (var userId in input.Users)
            {
                var user =  await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
                users.Add(user);
            }
        }
        await _context.SaveChangesAsync();
        return chatRoom;
    }
}