using System.Diagnostics.CodeAnalysis;

namespace SmartGrowHub.Domain.Extensions;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class OptionTExtensions
{
    public static IO<A> ReduceTransformer<A>(this OptionT<IO, A> option, Error error) =>
        option.ReduceTransformer(() => IO.fail<A>(error));
        
    public static IO<A> ReduceTransformer<A>(this OptionT<IO, A> option, Func<IO<A>> None) =>
        option.Match(Some: IO.pure, None: None).As().Flatten();
}