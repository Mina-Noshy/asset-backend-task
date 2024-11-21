using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Asset.Domain.Utilities
{
    public static class ConfigurationHelper
    {
        private static IConfiguration _configuration;
        private static IWebHostEnvironment _environment;
        public static void Initialize(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public static string GetConnectionString()
        {
            var connectionStringKey = _environment.IsDevelopment() ? "DevCS" : "LiveCS";

            return _configuration[$"ConnectionStrings:{connectionStringKey}"] ?? string.Empty;
        }

        public static string GetSerilog(string key)
            => _configuration[$"Serilog:{key}"] ?? string.Empty;

        public static string GetRateLimiter(string key)
            => _configuration[$"RateLimiter:{key}"] ?? string.Empty;

        public static string GetSMTP(string key)
            => _configuration[$"SMTP:{key}"] ?? string.Empty;

        public static string GetJWT(string key)
            => _configuration[$"JWT:{key}"] ?? string.Empty;

        public static string GetCORS(string key)
            => _configuration[$"CORS:{key}"] ?? string.Empty;

        public static string GetURL(string key)
         => _configuration[$"URLs:{key}"] ?? string.Empty;

        public static string GetProfile(string key)
            => _configuration[$"Profile:{key}"] ?? string.Empty;
    }
}
