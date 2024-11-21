namespace Asset.Application.Common;

public enum ResultType
{
    Success = 1000,
    Failure = 2000,
    Warning = 3000,
    Information = 4000,
    ValidationErrors = 5000,
    Exception = 6000,
    RateLimited = 7000,
    UnAuthorized = 8000,
    AccessDenied = 9000,
}
