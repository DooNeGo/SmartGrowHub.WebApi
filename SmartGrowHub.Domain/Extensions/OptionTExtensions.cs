using System.Diagnostics.CodeAnalysis;

namespace SmartGrowHub.Domain.Extensions;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class OptionTExtensions
{
    public static IO<A> ToIOOrFail<A>(this OptionT<IO, A> option, Error error) =>
        option.ToIOOrFail(() => IO.fail<A>(error));
        
    public static IO<A> ToIOOrFail<A>(this OptionT<IO, A> option, Func<IO<A>> None) =>
        option.Match(Some: IO.pure, None: None).As().Flatten();
}