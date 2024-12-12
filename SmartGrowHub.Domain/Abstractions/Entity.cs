using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Domain.Abstractions;

public abstract class Entity<T>(Id<T> id) : IEquatable<Entity<T>>
{
    public Id<T> Id { get; } = id;

    public bool Equals(Entity<T>? other) =>
        other is not null && Id.Equals(other.Id);

    public override bool Equals(object? obj) =>
        obj is Entity<T> entity && Id.Equals(entity.Id);

    public override int GetHashCode() => Id.GetHashCode();
}
