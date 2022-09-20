using AutoMapper;
using Identity.Configuration;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApi.Common.Constants;
using WebApi.Securities.Authorization;
using WebApplication1.Common.Attributes;
using WebApplication1.Database;
using WebApplication1.Entities;
using WebApplication1.Input;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers;


[ApiController]
public class MessageController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly JwtConfiguration _jwtOption;
    private readonly ICurrentAccountServices _currentAccount;
    private readonly IMessageService _messageService;

    public MessageController(ApplicationDbContext context,  IOptions<JwtConfiguration> jwtOption, ICurrentAccountServices currentAccount, IMessageService messageService)
    {
        _context = context;
        _jwtOption = jwtOption.Value;
        _currentAccount = currentAccount;
        _messageService = messageService;
    }

    
    
    
    [HttpPut]
    [Route("/message/{id}")]
    public async Task<MessageEntity> UpdateMessage(UpdateMessageInput input , Guid id)
    {
        var message = await  _context.Messages.FirstOrDefaultAsync(x => x.Id == id);
        var listLiker = new List<UserEntity>();
        if (input.Liker.Count > 0)
        {
            foreach (var userId in input.Liker)
            {
                var user =  await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
                listLiker.Add(user);
            }
        }
        message.Content = input.Content;
        message.Liker = listLiker;
        _context.SaveChanges();

        return message;

    }
    
    [HttpPost]
    [Route("/{ChatId}/message")]
    public async Task<MessageEntity> CreateMessage(CreateMessageInput input, Guid ChatId)
    {
        var message = new MessageEntity()
        {
            Id = Guid.NewGuid(),
            ChatRoom = ChatId,
            Content = input.Content,
            CreateTime = DateTimeOffset.UtcNow,
            Sender = _currentAccount.Id,
            FileAttach = "https://joeschmoe.io/api/v1/random"
        };
        await _context.AddAsync(message);
        await _context.SaveChangesAsync();
        return message;
    }
    [HttpGet]
    [Test("SUPPERADMIN")]
    [Route("/{ChatId}/message")]
    public async Task<IActionResult> ViewMessages(Guid ChatId,ViewListMessageInput input , CancellationToken cancellationToken)
    {
        // var messages = _context.Messages.Where(x => x.ChatRoom == ChatId).OrderBy(x=>x.CreateTime);
        var message = await _messageService.GetMessage(ChatId,input, cancellationToken);
        return Ok(message);
    }
}