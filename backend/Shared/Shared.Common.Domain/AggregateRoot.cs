namespace Shared.Common.Domain;

/// <summary>
/// Base class for aggregate roots (entities that serve as entry points to aggregates)
/// </summary>
public abstract class AggregateRoot : BaseEntity, IAggregateRoot
{
}
