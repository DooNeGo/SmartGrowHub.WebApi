namespace SmartGrowHub.Domain.Common;

public readonly record struct Id<T>(Ulid Value) : DomainType<Id<T>, Ulid>, DomainType<Id<T>, string>
{
    public Id() : this(Ulid.NewUlid()) { }
    
    public static implicit operator Ulid(Id<T> id) => id.Value;
    public static explicit operator Id<T>(Ulid ulid) => new(ulid);

    public static Fin<Id<T>> From(Ulid repr) => new Id<T>(repr);

    public Ulid To() => Value;

    public static Fin<Id<T>> From(string repr) =>
        Ulid.TryParse(repr, out Ulid id)
            ? From(id)
            : Error.New("This is an invalid ulid");

    string DomainType<Id<T>, string>.To() => ToString();

    public override string ToString() => Value.ToString();
}

public static class Id
{
    public static Id<T> New<T>() => new(Ulid.NewUlid());
}