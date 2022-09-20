using System.Reflection;
using System.Text.Json;
using FluentValidation.AspNetCore;
using Identity.Configuration;
using Microsoft.EntityFrameworkCore;
using WebApplication1;
using WebApplication1.Database;
using WebApplication1.Hubs;
using WebApplication1.Services;
using WebApplication1.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.FeatureManagement;
using WebApi.Securities.Authorization.PolicyProviders;
using WebApplication1.Securities.Authorization.Handlers;
using WebApplication1.Securities.Authorization.Requirements;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// validate 
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    })
    .AddFluentValidation(options =>
        {
            options.ImplicitlyValidateChildProperties = true;
            options.ImplicitlyValidateRootCollectionElements = true;
            // Automatic registration of validators in assembly
            options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }
    );
;

if (OperatingSystem.IsMacOS() || OperatingSystem.IsWindows() || OperatingSystem.IsLinux())
{
    Console.WriteLine($"✅ ✅ ✅ ✅ ✅ Running on customized ConfigureKestrel");
    Console.WriteLine($"⚙️⚙️⚙️⚙️⚙️ ENV: {builder.Environment.EnvironmentName}");
    builder.WebHost.ConfigureKestrel(options =>
    {
        // Force grpc work without tls
        options.ListenAnyIP(5023, config => { config.Protocols = HttpProtocols.Http2; });
        // Force http work with bot http1 and http2
        options.ListenAnyIP(5022, config => { config.Protocols = HttpProtocols.Http1AndHttp2; });
    });
}

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddCors(options => options.AddPolicy("CorsPolicy",
    builder =>
    {
        builder
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins("*");
    }));
// config database
var appDb = builder.Configuration.GetSection("AppDb").Get<AppDbConfiguration>();
var jwtConfig = builder.Configuration.GetSection("JwtConfiguration").Get<JwtConfiguration>();

builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
{
    options.UseNpgsql($"Server={appDb.Server};Port={appDb.Port};User Id={appDb.UserName};Password={appDb.Password};Database={appDb.Database}");
    options.UseLoggerFactory(LoggerFactory.Create(loggingBuilder => { loggingBuilder.AddConsole(); }
    ));
});
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddSingleton<IMessageService, MessageService>();
builder.Services.AddSingleton<IPaginationService, PaginationService>();
builder.Services.AddSingleton<ApplicationDbContext, ApplicationDbContext>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionsPolicyProvider>();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionsAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, RolesAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, ScopesAuthorizationHandler>();
builder.Services.AddSingleton<ICurrentAccountServices, CurrentAccountService>();

builder.Services.AddFeatureManagement();
builder.Services.Configure<JwtConfiguration>(
    builder.Configuration.GetSection(nameof(JwtConfiguration)));

builder.Services.AddWebApi(builder.Configuration, builder.Environment);
builder.Services.AddAuthentication();

builder.Services.AddAuthorization(option =>
{
    option.AddPolicy("AdminOnly", policy => policy.Requirements.Add(new MinimumAgeRequirement(0)));
    
});
builder.Services.AddSingleton<IAuthorizationHandler, MinimumAgeHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionsPolicyProvider>();
builder.Services.AddSignalR();

var app = builder.Build();


app.UseAuthentication();
app.UseAuthorization();


app.UseCors("CorsPolicy");
app.MapControllers();

app.MapHub<ChatHubs>("/chatHub");
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
        context.Response.Redirect("/swagger/index.html");
    else
        await next.Invoke();
});
app.Run();