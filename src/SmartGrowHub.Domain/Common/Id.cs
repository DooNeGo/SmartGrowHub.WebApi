namespace SmartGrowHub.Domain.Common;

public readonly record struct Id<T>(Ulid Value)
{
    public Id() : this(Ulid.NewUlid()) { }
    
    public static implicit operator Ulid(Id<T> id) => id.Value;

    public static Fin<Id<T>> From(string representation) =>
        Ulid.TryParse(representation, out Ulid ulid)
            ? new Id<T>(ulid)
            : Error.New("Invalid ulid representation");

    public override string ToString() => Value.ToString();
}