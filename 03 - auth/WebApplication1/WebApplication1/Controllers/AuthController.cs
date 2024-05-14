using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Database;
using WebApplication1.Dtos;
using WebApplication1.Models;
using WebApplication1.Services.Interfaces;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers;


[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly IVerificationCodeServices _codeService;

    public AuthController(IConfiguration configuration, AppDbContext dbContext, HttpClient? httpClient, IVerificationCodeServices codeService)
    {
        _configuration = configuration;
        _dbContext = dbContext;
        _codeService = codeService;
    }

    [HttpPost("register")]
    public ActionResult<string> Register(UserDto request)
    {
        if (_dbContext.Users.Any(x => x.Username == request.Username)) return BadRequest("User already exists");
        
        var user = new User
        {
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Username = request.Username
        };
        
        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();
        
        return Ok(CreateJwt(user, false));
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(UserDto request)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == request.Username);
        
        if (user == null) return BadRequest("User not found.");
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash)) return BadRequest("Wrong password.");

        return Ok(CreateJwt(user, false));
    }
    
    [Authorize]
    [HttpDelete("delete")]
    public async Task<ActionResult<string>> Delete(PasswordDto request)
    {
        var username = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);
    
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return BadRequest("Invalid username or password.");
        

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("forgotpass")]
    public async Task<ActionResult> ForgotPass(ForgotPassDto dto)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == dto.Username);
        
        if (user == null) return BadRequest("User Not Found!");

        using (HttpClient _httpClient = new HttpClient())
        {

            Uri url = new Uri("https://api.gzappy.com/v1/message/send-message");
            var userToken = GetUserToken();
            var message = new List<string> { "Your verification code is: " + _codeService.GenerateCode(dto.Username) };
            var phone = new List<string> { "+55" + dto.Username };

            WppMessage content = new WppMessage
            {
                instance_id = GetInstance_id(),
                instance_token = GetInstance_token(),
                message = message,
                phone = phone
            };

            HttpContent httpContent = JsonContent.Create(content);
            _httpClient.DefaultRequestHeaders.Add("user_token_id", userToken);
            RestoreConnection();
            //  HttpResponseMessage respose = await _httpClient.PostAsync(url, httpContent);
            //
            //  if (!respose.IsSuccessStatusCode)
            //  {
            //      string errorMessage = await respose.Content.ReadAsStringAsync();
            //      return BadRequest(errorMessage);
            //  }
            //
            // return Ok();
            return Ok(httpContent.ReadAsStringAsync());
        }
    }

    [HttpPost("verifycode")]
    public async Task<ActionResult> VerifyCode(VerifyCodeDto dto)
    {
        if (!_codeService.IsValidCode(dto.Username, dto.Code)) return BadRequest("Invalid Code or username");
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == dto.Username);
        return Ok(CreateJwt(user, true));
    }

    [Authorize]
    [HttpPost("resetpass")]
    public async Task<ActionResult> ResetPassword(PasswordDto dto)
    {
        var username = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
        if (HttpContext.User.FindFirst(ClaimTypes.AuthorizationDecision)?.Value != "True") return BadRequest("First request a password reset token");
        
        var user = await _dbContext.Users.FirstOrDefaultAsync(a => a.Username == username);
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        _dbContext.Update(user);
        await _dbContext.SaveChangesAsync();
        return Ok();
    }


    private async Task RestoreConnection()
    {
        using (HttpClient _httpClient = new HttpClient())
        {
            Uri url = new Uri("https://api.gzappy.com/v1/instances/restore");
            var userToken = GetUserToken();

            WppMessage content = new WppMessage
            {
                instance_id = GetInstance_id(),
                instance_token = GetInstance_id(),
            };

            HttpContent httpContent = JsonContent.Create(content);
            _httpClient.DefaultRequestHeaders.Add("user_token_id", userToken);
            await _httpClient.PatchAsync(url, httpContent).ConfigureAwait(false);
        }
    }
    private string CreateJwt(User user, bool passReset)
    {
        var claims = new List<Claim>{
            new Claim(ClaimTypes.Name, user.Username)
        };
        if (passReset)
        {
            claims.Add(new Claim(ClaimTypes.AuthorizationDecision, passReset.ToString()));
        }

        var signinKey = GetSigninKey();
        var issuer = GetIssuer();
        var audience = GetAudience();

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signinKey)),
            SecurityAlgorithms.HmacSha256
        );
        
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(2),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }
    private string? GetAudience()
    {
        return _configuration.GetValue<string>("Jwt:Audience");
    }

    private string? GetIssuer()
    {
        return _configuration.GetValue<string>("Jwt:Issuer");
    }

    private string? GetSigninKey()
    {
        return _configuration.GetValue<string>("Jwt:Key");
         
    }

    private string? GetUserToken()
    {
        return _configuration.GetValue<string>("Gzappy:User_token_id");
    }

    private string? GetInstance_id()
    {
        return _configuration.GetValue<string>("Gzappy:Instance_id");
    }

    private string? GetInstance_token()
    {
        return _configuration.GetValue<string>("Gzappy:Instance_token");
    }
}