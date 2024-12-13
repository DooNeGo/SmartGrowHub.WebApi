using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Application.Services;

public interface IOtpIssuer
{
    Eff<OneTimePassword> Create(Id<User> id);
    
    TimeSpan OtpLifetime { get; }
}