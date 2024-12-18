using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Application.UseCases.Auth;

public sealed class SendOtpToEmailUseCase(
    IEmailService emailService,
    IOtpIssuer otpIssuer,
    IOtpRepository otpRepository,
    IUserRepository userRepository,
    IEmailTemplateService emailTemplateService)
{
    private const string Subject = "One Time Password for Smart Grow Hub";
    
    public Eff<Unit> SendOtpToEmail(EmailAddress emailAddress, CancellationToken cancellationToken) =>
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
        userRepository
            .GetByEmailAddress(emailAddress, cancellationToken)
            .IfFailEff(_ => userRepository
                .Add(User.NewFromEmailAddress(emailAddress), cancellationToken)
                .Bind(_ => userRepository.GetByEmailAddress(emailAddress, cancellationToken)));
}