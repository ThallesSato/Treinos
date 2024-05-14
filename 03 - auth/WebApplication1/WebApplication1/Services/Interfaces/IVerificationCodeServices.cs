namespace WebApplication1.Services.Interfaces;

public interface IVerificationCodeServices
{
    public string GenerateCode(string username);
    public bool IsValidCode(string username, string code);
}