namespace SmartGrowHub.Domain.Common;

public interface IId<out T>;

// TODO: Сделать реализацию Id как discriminated union,
// TODO: чтобы иметь UlidId, StringId, IntId, GrowHubId... А на уровне ef сделать IdConverter

public sealed class Id<T> : DomainType<Id<T>, string>
{
    private Id(string value) => Value = value;
    
    public Id() : this(Ulid.NewUlid().ToString()) { }

    public static implicit operator string(Id<T> id) => id.Value;
    
    public string Value { get; }

    public static Fin<Id<T>> From(string repr) =>
        Ulid.TryParse(repr, out Ulid _)
            ? new Id<T>(repr)
            : Error.New("Invalid ulid representation");

    public string To() => Value;
    
    public override string ToString() => Value;
}