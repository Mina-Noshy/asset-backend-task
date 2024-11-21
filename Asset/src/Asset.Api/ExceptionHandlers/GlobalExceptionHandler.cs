using Asset.Application.Common;
using Asset.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using System.Net;
using System.Text.Json;

namespace Asset.Api.ExceptionHandlers;

public class GlobalExceptionHandler : IExceptionHandler // (IProblemDetailsService _problemDetailsService)
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.ContentType = "application/json";

        var excDetails = exception switch
        {
            ValidationAppException => (
                Detail: exception.Message,
                StatusCode: StatusCodes.Status422UnprocessableEntity),
            _ => (
                Detail: exception.Message,
                StatusCode: (int)HttpStatusCode.InternalServerError)
        };

        // Return validation message
        httpContext.Response.StatusCode = excDetails.StatusCode;
        if (exception is ValidationAppException validationAppException)
        {
            var validationExceptionResponse =
                new ApiResponseContract(ResultType.ValidationErrors, exception.Message, validationAppException.Errors);

            await httpContext.Response.WriteAsJsonAsync(validationExceptionResponse);
            return true;
        }

        // Log the exception
        Log.Error(exception, excDetails.Detail);

        // Return exception message
        var response = new ApiResponseContract(ResultType.Exception, exception.Message);
        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
        return true;

        //return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext()
        //{
        //    HttpContext = httpContext,
        //    ProblemDetails =
        //    {
        //        Title = "An error occurred",
        //        Detail = excDetails.Detail,
        //        Type = exception.GetType().Name,
        //        Status = excDetails.StatusCode
        //    },
        //    Exception = exception
        //});

    }

}
