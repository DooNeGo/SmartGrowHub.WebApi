using System.Diagnostics.CodeAnalysis;

namespace SmartGrowHub.Domain.Extensions;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class IOExtensions
{
    public static IO<A> TapOnFail<A, B>(this IO<A> io, Func<Error, IO<B>> func) =>
        io.IfFail(error => func(error).Bind(_ => IO.fail<A>(error)));
    
    public static IO<Unit> ToUnit<T>(this IO<T> io) => io.Kind().ToUnit().As();
}