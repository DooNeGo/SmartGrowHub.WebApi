using System.Text.Json.Serialization;
using SmartGrowHub.Shared.Auth;
using SmartGrowHub.Shared.GrowHubs.Model;
using SmartGrowHub.Shared.GrowHubs.Requests;
using SmartGrowHub.Shared.Results;
using SmartGrowHub.Shared.Tokens;
using SmartGrowHub.Shared.Users;

namespace SmartGrowHub.Shared.SerializerContext;

[JsonSerializable(typeof(LogInByEmailRequest))]
[JsonSerializable(typeof(LogInByPhoneRequest))]
[JsonSerializable(typeof(RefreshTokensRequest))]
[JsonSerializable(typeof(CheckOtpRequest))]
[JsonSerializable(typeof(SetModuleProgramRequestDto))]
[JsonSerializable(typeof(Result))]
[JsonSerializable(typeof(Result<IEnumerable<AuthTokensDto>>))]
[JsonSerializable(typeof(Result<IEnumerable<UserDto>>))]
[JsonSerializable(typeof(Result<IEnumerable<GrowHubDto>>))]
public sealed partial class SmartGrowHubSerializerContext : JsonSerializerContext;