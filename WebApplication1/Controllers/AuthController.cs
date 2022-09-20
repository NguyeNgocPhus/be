using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Google.Protobuf;
using Identity.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WebApi.Securities.Authorization;
using WebApplication1.Database;
using WebApplication1.Entities;
using WebApplication1.Input;
using WebApplication1.Securities.Authorization;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers;

public class WeatherForecast
{
    public DateTimeOffset Date { get; set; }
    public int TemperatureCelsius { get; set; }
    public string? Summary { get; set; }
}

[ApiController]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly JwtConfiguration _jwtOption;
    private readonly ICurrentAccountServices _currentAccount;
    public AuthController(ApplicationDbContext context, IOptions<JwtConfiguration> jwtOptionn, ICurrentAccountServices currentAccount)
    {
        _context = context;
        _currentAccount = currentAccount;
        _jwtOption = jwtOptionn.Value;
    }

    [HttpGet]
    [Route("/user/{id}")]
    public async Task<UserEntity> GetUserById(Guid id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        return user;
    }
    [HttpPost]
    [Route("/register")]
    [MinimumAgeAuthorize(new[]{1})]
    public async Task<IActionResult> CreateUser(CreateUserInput input)
    {
        // var user =  new UserEntity()
        // {
        //     Id =Guid.NewGuid(),
        //     Name = input.Name,
        //     Password = input.Password,
        //     Email = input.Email
        // };
        // await _context.AddAsync(user);
        // 
        var account = JsonConvert.DeserializeObject<WeatherForecast>("{\"Date\":\"2019-08-01T00:00:00+07:00\",\"TemperatureCelsius\":\"1\",\"Summary\":\"Hot\"}");
// //var b = JsonSerializer.Deserialize<WeatherForecast>("{\"Date\":\"2019-08-01T00:00:00+07:00\",\"TemperatureCelsius\":\"25\",\"Summary\":\"Hot\"}");
        Console.WriteLine(account);
        return Ok(account);
    }
    [HttpPost]
    [Route("/auth/login")]
    public async Task<string> LoginUser(LoginUserInput input)
    {
        // var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == input.Email && x.Password == input.Password);
        // if (user == null)
        // {
        //     throw new Exception("sai mm rooi") ;
        // }
        var claims = new[]
        {
            new Claim(ClaimTypes.Email, "phu@gmail.com"),
            new Claim(ClaimTypes.Name, "Phu"),
            new Claim("uid", Guid.NewGuid().ToString()), 
            new Claim("permissions", "SUPPERADMIN")
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtOption.SymmetricSecurityKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = _jwtOption.Audience,
            Issuer = _jwtOption.Issuer,
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(_jwtOption.Expires),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };
        await Task.CompletedTask;
        var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

        return token;
    }

    [HttpGet]
    // [Authorize(Policy = "AdminOnly")]
    [Permissions(Permissions = new []{"admin"})]
    [Route("/myprofile")]
    public async Task<IActionResult?> GetMyProfile()
    {
    //     var userId = _currentAccount.Id;
    //     var user = _context.Users.FirstOrDefault(x => x.Id == userId);
        var account = JsonConvert.DeserializeObject("{\"Date\":\"2019-08-01T00:00:00+07:00\",\"TemperatureCelsius\":\"1\",\"Summary\":\"Hot\"}");
// //var b = JsonSerializer.Deserialize<WeatherForecast>("{\"Date\":\"2019-08-01T00:00:00+07:00\",\"TemperatureCelsius\":\"25\",\"Summary\":\"Hot\"}");
        Console.WriteLine(account);
        return Ok(account);
    }

    [HttpPost]
    [Route("/uploadFile")]
    [FeatureGate("ok")]
    public async Task<IActionResult> UpLoadFileAync(IFormFile file)
    {
        var path = Path.Combine(@"D:\C#\WebApplication1\WebApplication1\Resources", file.FileName);
        await using var stream = new FileStream(path, FileMode.Create);
        await file.CopyToAsync(stream); //save the file
        
        var data =await ByteString.FromStreamAsync(file.OpenReadStream());
        var dataFile = data.ToByteArray();
        // var dataFile = Encoding.UTF8.GetBytes(file.OpenReadStream().ToString());
        Random rnd = new Random();
        var fileUpdate = new FileUpdateReadModel()
        {
            Id = Guid.NewGuid(),
            Code = rnd.Next(1000),
            Data = dataFile,
            MimeType = file.ContentType,
            Name = file.FileName,
            Size = file.Length,
            OriginalName = file.FileName


        };
        await _context.AddAsync(fileUpdate);
        await _context.SaveChangesAsync();
        return Ok(fileUpdate);
    }

    [HttpGet]
    [Route("/fileUpload/{fileName}")]
    public async Task<IActionResult> GetFileAsync(string fileName)
    {
        
        var appSettingsPath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "appsettings.json");
        var json = System.IO.File.ReadAllText(appSettingsPath);
        
        
        var jsonSettings = new JsonSerializerSettings();
        jsonSettings.Converters.Add(new ExpandoObjectConverter());
        jsonSettings.Converters.Add(new StringEnumConverter());
        
        var config = JsonConvert.DeserializeObject<ExpandoObject>(json, jsonSettings);
        foreach (var (data, value) in config)
        {
            if (data == "FeatureManagement")
            {
                
            }
        }

        var newJson = JsonConvert.SerializeObject(config, Formatting.Indented, jsonSettings);

        var file = await _context.FileUpdates.FirstOrDefaultAsync(x => x.Name == fileName);
        var response = new FileContentResult(ByteString.CopyFrom(file.Data).ToByteArray(), file.MimeType)
        {
            // FileDownloadName = file.Name,
            EnableRangeProcessing = true
        };
        // string path = Path.Combine(_webHostEnvironment.ContentRootPath, _fileConfiguration.ExportFolder, fileName);

        var response1 = new PhysicalFileResult(@"D:\C#\WebApplication1\WebApplication1\Resources\file", "application/pdf");
        response1.FileDownloadName = fileName;
        response1.EnableRangeProcessing = true; // Allow browser to know len of file
        
        return response1;
    }
}