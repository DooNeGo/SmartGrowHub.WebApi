using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Application.UseCases.Auth;

public sealed class RequestOtpToEmailUseCase(
    IEmailService emailService,
    IOtpIssuer otpIssuer,
    IOtpRepository otpRepository,
    IUserRepository userRepository,
    IEmailTemplateService emailTemplateService)
{
    private const string Subject = "One Time Password for Smart Grow Hub";

    public IO<Unit> RequestOtpToEmail(EmailAddress emailAddress) =>
        from user in GetOrCreateUserByEmail(emailAddress)
        from oneTimePassword in otpIssuer.Create(user.Id)
        from subject in NonEmptyString.From(Subject).ToIO()
        from body in emailTemplateService.GetOtpEmailBody(oneTimePassword.Value, otpIssuer.OtpLifetime)
        from _1 in emailService.Send(emailAddress, subject, body, isHtmlBody: true)
        from _2 in otpRepository.AddAndSave(oneTimePassword)
        select unit;

    private IO<User> GetOrCreateUserByEmail(EmailAddress emailAddress) =>
        userRepository
            .GetByEmailAddress(emailAddress)
            .ToIOOrFail(() =>
                from user in IO.pure(User.NewFromEmailAddress(emailAddress))
                from _ in userRepository.AddAndSave(user)
                select user);
}