using Asset.Domain.Entities.Shared;

namespace Asset.Domain.Exceptions;

public sealed class EntityNotFoundException<T> : NotFoundException where T : TEntity
{
    public EntityNotFoundException(long id)
        : base($"The entity '{typeof(T).Name}' with the identifier '{id}' was not found.")
    {
    }
}
