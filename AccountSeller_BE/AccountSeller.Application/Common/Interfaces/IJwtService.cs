﻿namespace AccountSeller.Application.Common.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(string subject, string claimType);
        string GetClaim(string token, string claimType);
        bool ValidateCurrentToken(string token);
    }
}