using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Application.UseCases.Auth;

public sealed class SendOtpToPhoneUseCase(
    ISmsService smsService,
    IOtpIssuer otpIssuer,
    IOtpRepository otpRepository,
    IUserRepository userRepository)
{
    public IO<Unit> SendCodeToPhone(PhoneNumber phoneNumber, CancellationToken cancellationToken) =>
        from user in GetOrCreateUserByPhone(phoneNumber, cancellationToken)
        from oneTimePassword in otpIssuer.Create(user.Id)
        from payload in NonEmptyString.From($"Your one time password: {oneTimePassword.Value}").ToIO()
        from _ in smsService.Send(phoneNumber, payload, cancellationToken)
        from __ in otpRepository.Add(oneTimePassword, cancellationToken)
        select unit;

    private IO<User> GetOrCreateUserByPhone(PhoneNumber phoneNumber, CancellationToken cancellationToken) =>
        userRepository
            .GetByPhoneNumber(phoneNumber, cancellationToken)
            .ReduceTransformer(() =>
                from user in IO.pure(User.NewFromPhoneNumber(phoneNumber))
                from _1 in userRepository.Add(user, cancellationToken)
                select user);
}