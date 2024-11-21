using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Asset.Application.Common;

public class ApiResponse : IActionResult
{
    private readonly HttpStatusCode _status = HttpStatusCode.OK;
    private readonly ResultType _code;
    private readonly object _data;
    private readonly string _message;

    public ApiResponse(ResultType _code, string _message) : this(_code, _message, string.Empty)
    {
    }

    public ApiResponse(ResultType _code, object _data) : this(_code, string.Empty, _data)
    {
    }

    public ApiResponse(ResultType Code, string Message, object Data)
    {
        _code = Code;
        _data = Data;
        _message = Message;
    }

    public async Task ExecuteResultAsync(ActionContext context)
    {
        var objectResult = new ObjectResult(new ApiResponseContract(_code, _message, _data))
        {
            StatusCode = (int)_status,
        };

        await objectResult.ExecuteResultAsync(context);
    }
}
