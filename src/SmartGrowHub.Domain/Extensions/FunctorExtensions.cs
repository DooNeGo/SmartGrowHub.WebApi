using System.Diagnostics.CodeAnalysis;

namespace SmartGrowHub.Domain.Extensions;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class FunctorExtensions
{
    public static K<F, Unit> ToUnit<F, A>(this K<F, A> fa) where F : Functor<F> => fa.Map(_ => Unit.Default);

    public static K<F, B> Cast<F, A, B>(this K<F, A> fa) where F : Functor<F> where A : B => fa.Map(B (a) => a);
}