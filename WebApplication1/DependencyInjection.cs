using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Common;

namespace WebApplication1;

public static class DependencyInjection
{
    public static IServiceCollection AddWebApi(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
        services.AddIdentityService(configuration);
        return services;
    }

    public static void AddIdentityService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(config =>
        {
            config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(config =>
        {
            config.RequireHttpsMetadata = false;
            config.SaveToken = true;
            config.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = configuration["JwtConfiguration:Audience"],
                ValidIssuer =  configuration["JwtConfiguration:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtConfiguration:SymmetricSecurityKey"]))
            };
            config.Events = new JwtBearerEvents()
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];
                    var token = context.HttpContext.Request.Headers;
                    // If the request is for our hub...
                    var path = context.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) &&
                        (path.StartsWithSegments("/notification/web")))
                    {
                        // Read the token out of the query string
                        context.Token = accessToken;
                    }

                    return Task.CompletedTask;
                },
                OnTokenValidated = async context =>
                {
                    // To check token is valid and must be existing in the UserToken table in the database
                    // Once JWT is not existing in the UserToken table, the authentication process will be set as failed.

                    var userIdClaim =
                        context.Principal?.Claims.FirstOrDefault(claim => claim.Type == JwtClaimTypes.UserId);
                    if (userIdClaim == null)
                    {
                        context.Fail("JWT Token does not contain User Id Claim.");
                    }
                    var token = context.HttpContext.Request.Headers["Authorization"].ToString()
                        .Replace("Bearer ", "");
                    // If we cannot get token from header, try to use from querystring (for wss)

                    // context.Fail("JWT Token does not contain User Id Claim.");
                    Console.WriteLine(@"Token Validated OK");
                },
                OnChallenge = context =>
                {
                    context.HandleResponse();
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";
                    // Ensure we always have an error and error description.
                    if (string.IsNullOrEmpty(context.Error))
                        context.Error = "invalid_token";
                    if (string.IsNullOrEmpty(context.ErrorDescription))
                    {
                        // Pass the message from OnTokenValidated on method context.Fail(<message>)
                        if (context.AuthenticateFailure != null &&
                            context.AuthenticateFailure.Message.Length > 0)
                        {
                            context.ErrorDescription = context.AuthenticateFailure.Message;
                        }
                        else
                        {
                            // If we dont have error message from OnTokenValidated, set a message
                            context.ErrorDescription =
                                "This request requires a valid JWT access token to be provided.";
                        }
                    }

                    // Add some extra context for expired tokens.
                    if (context.AuthenticateFailure != null && context.AuthenticateFailure.GetType() ==
                        typeof(SecurityTokenExpiredException))
                    {
                        var authenticationException =   
                            context.AuthenticateFailure as SecurityTokenExpiredException;
                        context.Response.Headers.Add("WWW-Authenticate", "Bearer");
                        context.ErrorDescription = $"The token expired on {authenticationException?.Expires:o}";
                    }

                    return context.Response.WriteAsync(JsonSerializer.Serialize(new
                    {
                        status = 401,
                        error = context.Error,
                        errorDescription = context.ErrorDescription
                    }));
                }
            };
        });
    }
}