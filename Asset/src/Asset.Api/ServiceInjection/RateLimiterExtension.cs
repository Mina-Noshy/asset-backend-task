using Asset.Application.Common;
using Asset.Domain.Utilities;
using System.Text.Json;
using System.Threading.RateLimiting;

namespace Asset.Api.ServiceInjection;

public static class RateLimiterExtension
{

    public static IServiceCollection AddRateLimiterExtension(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
                    factory: partition => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = int.Parse(ConfigurationHelper.GetRateLimiter("Requests")),
                        QueueLimit = 0,
                        Window = TimeSpan.FromSeconds(int.Parse(ConfigurationHelper.GetRateLimiter("Duration")))
                    }));

            options.OnRejected = async (context, token) =>
            {
                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;

                var retryAfterSeconds = context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter) ? retryAfter.TotalSeconds : (double?)null;
                var msg = $"Too many requests. Please try again after {retryAfter.TotalSeconds} seconds(s).";

                var response = new ApiResponseContract(ResultType.RateLimited, msg, string.Empty);

                var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                });

                await context.HttpContext.Response.WriteAsync(jsonResponse, cancellationToken: token);

            };
        });

        return services;
    }

}
