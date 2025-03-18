using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Extensions;
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

    public IO<Unit> SendOtpToEmail(EmailAddress emailAddress, CancellationToken cancellationToken) =>
        from user in GetOrCreateUserByEmail(emailAddress, cancellationToken)
        from oneTimePassword in otpIssuer.Create(user.Id)
        from subject in NonEmptyString.From(Subject).ToIO()
        from body in emailTemplateService.GetOtpEmailBody(
            oneTimePassword.Value, otpIssuer.OtpLifetime, cancellationToken)
        from _ in emailService
            .To(emailAddress)
            .Subject(subject)
            .Body(body, isHtml: true)
            .Send(cancellationToken)
        from __ in otpRepository.Add(oneTimePassword, cancellationToken)
        select unit;

    private IO<User> GetOrCreateUserByEmail(EmailAddress emailAddress, CancellationToken cancellationToken) =>
        userRepository
            .GetByEmailAddress(emailAddress, cancellationToken)
            .ReduceTransformer(() =>
                from user in IO.pure(User.NewFromEmailAddress(emailAddress))
                from _1 in userRepository.Add(user, cancellationToken)
                select user);
}