namespace LifeTrack.Core.Models.Contracts.API.Auth;

public record RegistrationRequest(string Username, string Password, string Email);