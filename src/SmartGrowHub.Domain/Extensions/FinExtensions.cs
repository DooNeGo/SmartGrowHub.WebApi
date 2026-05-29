using System.Diagnostics.CodeAnalysis;

namespace SmartGrowHub.Domain.Extensions;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class FinExtensions
{
    public static IO<A> ToIO<A>(this Fin<A> fin) => fin.Match(Succ: IO.pure, Fail: IO.fail<A>);
    
    public static Fin<B> Cast<A, B>(this Fin<A> fa) where A : B => fa.Cast<Fin, A, B>().As();
}