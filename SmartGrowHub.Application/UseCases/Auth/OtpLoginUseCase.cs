using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Application.UseCases.Auth;

public sealed class OtpLoginUseCase(
    IEmailService emailService,
    ISmsService smsService,
    IOtpIssuer otpIssuer,
    IOtpRepository otpRepository,
    IUserRepository userRepository,
    IEmailTemplateService emailTemplateService)
{
    private const string Subject = "One Time Password for Smart Grow Hub";
    
    public Eff<Unit> SendCodeToEmail(EmailAddress emailAddress, CancellationToken cancellationToken) =>
        from user in GetOrCreateUserByEmail(emailAddress, cancellationToken)
        from oneTimePassword in otpIssuer.Create(user.Id)
        from subject in NonEmptyString.From(Subject).ToEff()
        from body in emailTemplateService.GetOtpEmailBody(oneTimePassword.Value, otpIssuer.OtpLifetime)
        from _ in emailService
            .To(emailAddress)
            .Subject(subject)
            .Body(body, isHtml: true)
            .Send(cancellationToken)
        from __ in otpRepository.Add(oneTimePassword, cancellationToken)
        select unit;

    private Eff<User> GetOrCreateUserByEmail(EmailAddress emailAddress, CancellationToken cancellationToken) =>
        userRepository.GetByEmailAddress(emailAddress, cancellationToken)
        | @catch(_ => userRepository
            .Add(User.NewFromEmailAddress(emailAddress), cancellationToken)
            .Bind(_ => userRepository.GetByEmailAddress(emailAddress, cancellationToken)));

    public Eff<Unit> SendCodeToPhone(PhoneNumber phoneNumber, CancellationToken cancellationToken) =>
        from user in GetOrCreateUserByPhone(phoneNumber, cancellationToken)
        from oneTimePassword in otpIssuer.Create(user.Id)
        from payload in NonEmptyString.From($"Your one time password: {oneTimePassword.Value}").ToEff()
        from _ in smsService.Send(phoneNumber, payload, cancellationToken)
        from __ in otpRepository.Add(oneTimePassword, cancellationToken)
        select unit;
    
    private Eff<User> GetOrCreateUserByPhone(PhoneNumber phoneNumber, CancellationToken cancellationToken) =>
        userRepository.GetByPhoneNumber(phoneNumber, cancellationToken)
        | @catch(_ => userRepository
            .Add(User.NewFromPhoneNumber(phoneNumber), cancellationToken)
            .Bind(_ => userRepository.GetByPhoneNumber(phoneNumber, cancellationToken)));
}