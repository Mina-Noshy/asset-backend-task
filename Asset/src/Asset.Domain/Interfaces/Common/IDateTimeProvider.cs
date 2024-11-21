namespace Asset.Domain.Interfaces.Common;

public interface IDateTimeProvider
{
    public DateTime CurrentDateTime { get; }
}
