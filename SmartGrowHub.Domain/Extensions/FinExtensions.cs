using System.Diagnostics.CodeAnalysis;

namespace SmartGrowHub.Domain.Extensions;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class FinExtensions
{
    public static IO<A> ToIO<A>(this Fin<A> fin) => fin.Match(Succ: IO.pure, Fail: IO.fail<A>);
}