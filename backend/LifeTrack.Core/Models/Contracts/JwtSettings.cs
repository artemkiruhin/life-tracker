namespace LifeTrack.Core.Models.Contracts;

public record JwtSettings(string Audience, string Issuer, string SecurityKey, int ExpiresInHours);