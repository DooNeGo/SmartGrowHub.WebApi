using SmartGrowHub.Shared.Auth.Dto.LogIn;
using SmartGrowHub.Shared.Auth.Dto.Register;
using SmartGrowHub.Shared.Users.Dto;
using System.Text.Json.Serialization;

namespace SmartGrowHub.WebApi.SerializerContext;

[JsonSerializable(typeof(UserDto))]
[JsonSerializable(typeof(LoginResponseDto))]
[JsonSerializable(typeof(LogInRequestDto))]
[JsonSerializable(typeof(RegisterRequestDto))]
public sealed partial class SmartGrowHubSerializerContext
    : JsonSerializerContext;