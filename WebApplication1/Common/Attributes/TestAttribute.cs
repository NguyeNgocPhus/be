using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Identity.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApi.Common.Constants;

namespace WebApplication1.Common.Attributes;


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class TestAttribute:Attribute,IAsyncActionFilter
{

    private readonly string _permission;

  
    public TestAttribute(string permission)
    {
        _permission = permission;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var _jwtOption = context.HttpContext.RequestServices
            .GetRequiredService<IOptions<JwtConfiguration>>();
        
        var token = context.HttpContext.Request.Headers["Authorization"].ToString()
            .Replace("Bearer ", "");
        var tokenHandler = new JwtSecurityTokenHandler();
        var b = _jwtOption.Value.Audience;
        var tokenDescriptor = new TokenValidationParameters  
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = _jwtOption.Value.Audience,
            ValidIssuer =  _jwtOption.Value.Issuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.Value.SymmetricSecurityKey))
        };
        SecurityToken validatedToken;
        var claims =  tokenHandler.ValidateToken(token, tokenDescriptor , out validatedToken);
        var userId = claims.Claims.First(claims => claims.Type.Equals(JwtClaimTypes.UserId)).Value;
        var permission = claims.Claims.First(claims => claims.Type.Equals("permissions")).Value;
        if (_permission != permission)
        {
            throw new Exception("khong dung permission");
        }
        var executedResult = await next();
        if (executedResult.Result is OkObjectResult okObjectResult)
        {
            // Cache the executed result
            
        }
    }
}