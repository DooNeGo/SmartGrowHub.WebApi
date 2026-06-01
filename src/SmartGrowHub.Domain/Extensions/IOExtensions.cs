using System.Diagnostics.CodeAnalysis;

namespace SmartGrowHub.Domain.Extensions;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class IOExtensions
{
    extension<A>(IO<A> io)
    {
        public IO<A> TapOnFail<B>(Func<Error, IO<B>> func) =>
            io.IfFail(error => func(error).Bind(_ => IO.fail<A>(error)));

        public IO<Unit> ToUnit() => io.Kind().ToUnit().As();
    }
}