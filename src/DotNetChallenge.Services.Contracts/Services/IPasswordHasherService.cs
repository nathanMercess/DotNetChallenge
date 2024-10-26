﻿namespace DotNetChallenge.Services.Contracts.Services;

public interface IPasswordHasherService
{
    public string HashPassword(string password);

    public bool VerifyPassword(string password, string hashedPassword);
}
