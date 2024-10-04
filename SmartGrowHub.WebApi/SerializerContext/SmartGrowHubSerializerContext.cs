using SmartGrowHub.Shared.Auth.Dto.LogIn;
using SmartGrowHub.Shared.Auth.Dto.Register;
using SmartGrowHub.Shared.Users.Dto;
using SmartGrowHub.Shared.UserSessions.Dto.RefreshTokens;
using System.Text.Json.Serialization;

namespace SmartGrowHub.WebApi.SerializerContext;

[JsonSerializable(typeof(UserDto))]
[JsonSerializable(typeof(LogInResponseDto))]
[JsonSerializable(typeof(LogInRequestDto))]
[JsonSerializable(typeof(RegisterRequestDto))]
[JsonSerializable(typeof(RefreshTokensRequestDto))]
[JsonSerializable(typeof(RefreshTokensResponseDto))]
public sealed partial class SmartGrowHubSerializerContext
    : JsonSerializerContext;