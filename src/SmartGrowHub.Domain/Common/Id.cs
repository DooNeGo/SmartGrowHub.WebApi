namespace SmartGrowHub.Domain.Common;

public interface IId<out T>;

// TODO: Сделать реализацию Id как discriminated union,
// TODO: чтобы иметь UlidId, StringId, IntId, GrowHubId... А на уровне ef сделать IdConverter

public sealed record Id<T>(Ulid Value) : DomainType<Id<T>, string>
{
    public Id() : this(Ulid.NewUlid()) { }

    public static implicit operator string(Id<T> id) => id.Value.ToString();
    public static implicit operator Ulid(Id<T> id) => id.Value;

    public static Fin<Id<T>> From(string repr) =>
        Ulid.TryParse(repr, out Ulid ulid)
            ? new Id<T>(ulid)
            : Error.New("Invalid ulid representation");

    public string To() => Value.ToString();
    
    public override string ToString() => Value.ToString();
}