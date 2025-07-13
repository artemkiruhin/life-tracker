namespace LifeTrack.Core.Models.Contracts.Auth;

public record RegistrationRequest(string Username, string Password, string Email);