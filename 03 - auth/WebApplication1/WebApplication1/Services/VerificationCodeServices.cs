using Microsoft.Extensions.Caching.Memory;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services;

public class VerificationCodeServices : IVerificationCodeServices
{
    private readonly IMemoryCache _memoryCache;

    public VerificationCodeServices(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public string GenerateCode(string username)
    {
        var code = Random.Shared.Next(100000, 999999).ToString();
        var cacheEntryOptions = new MemoryCacheEntryOptions
        {
            Size = 1, 
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(3)
        };

        _memoryCache.Set(username, code, cacheEntryOptions);
        return code;
    }

    public bool IsValidCode(string username, string code)
    {
        return _memoryCache.TryGetValue(username, out string storedCode) && storedCode == code;
    }
}