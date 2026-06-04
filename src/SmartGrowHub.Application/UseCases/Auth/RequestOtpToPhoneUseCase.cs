using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Application.UseCases.Auth;

public sealed class RequestOtpToPhoneUseCase(
    ISmsService smsService,
    IOtpIssuer otpIssuer,
    IOtpRepository otpRepository,
    IUserRepository userRepository)
{
    public IO<Unit> RequestOtpToPhone(PhoneNumber phoneNumber) =>
        from user in GetOrCreateUserByPhone(phoneNumber)
        from oneTimePassword in otpIssuer.Create(user.Id)
        from payload in NonEmptyString.From($"Your one time password: {oneTimePassword.Value}").ToIO()
        from _1 in smsService.Send(phoneNumber, payload)
        from _2 in otpRepository.AddAndSave(oneTimePassword)
        select unit;

    private IO<User> GetOrCreateUserByPhone(PhoneNumber phoneNumber) =>
        userRepository
            .GetByPhoneNumber(phoneNumber)
            .ToIOOrFail(() =>
                from user in IO.pure(User.NewFromPhoneNumber(phoneNumber))
                from _ in userRepository.AddAndSave(user)
                select user);
}