using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Application.UseCases.Auth;

public sealed record LoginRequest(Either<EmailAddress, PhoneNumber> EmailOrPhone);