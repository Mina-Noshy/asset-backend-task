using Asset.Domain.Interfaces.Common;

namespace Asset.Domain.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime CurrentDateTime => DateTime.UtcNow;
}
