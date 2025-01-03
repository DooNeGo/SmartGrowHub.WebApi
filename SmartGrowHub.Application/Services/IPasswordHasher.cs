﻿using SmartGrowHub.Domain.Common.Password;

namespace SmartGrowHub.Application.Services;

public interface IPasswordHasher
{
    Fin<HashedPassword> Hash(PlainTextPassword password);
    Fin<bool> Verify(Password password1, Password password2);
}