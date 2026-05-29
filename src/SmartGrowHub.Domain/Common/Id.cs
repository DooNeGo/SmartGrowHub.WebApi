namespace SmartGrowHub.Domain.Common;

public interface IId<out T>;

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

// public readonly record struct Id<T>(Ulid Value)
// {
//     public Id() : this(Ulid.NewUlid()) { }
//     
//     public static implicit operator Ulid(Id<T> id) => id.Value;
//
//     public static Fin<Id<T>> From(string representation) =>
//         Ulid.TryParse(representation, out Ulid ulid)
//             ? new Id<T>(ulid)
//             : Error.New("Invalid ulid representation");
//
//     public override string ToString() => Value.ToString();
// }