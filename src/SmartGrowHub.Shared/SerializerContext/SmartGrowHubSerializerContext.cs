using System.Text.Json.Serialization;
using SmartGrowHub.Shared.Auth;
using SmartGrowHub.Shared.GrowHubs;
using SmartGrowHub.Shared.Results;
using SmartGrowHub.Shared.Tokens;
using SmartGrowHub.Shared.Users;

namespace SmartGrowHub.Shared.SerializerContext;

[JsonSerializable(typeof(LogInByEmailRequest))]
[JsonSerializable(typeof(LogInByPhoneRequest))]
[JsonSerializable(typeof(RefreshTokensRequest))]
[JsonSerializable(typeof(CheckOtpRequest))]
[JsonSerializable(typeof(Result))]
[JsonSerializable(typeof(Result<AuthTokensDto>))]
[JsonSerializable(typeof(Result<UserDto>))]
[JsonSerializable(typeof(Result<IEnumerable<GrowHubDto>>))]
public sealed partial class SmartGrowHubSerializerContext : JsonSerializerContext;