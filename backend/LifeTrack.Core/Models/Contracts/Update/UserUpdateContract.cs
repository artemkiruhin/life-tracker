namespace LifeTrack.Core.Models.Contracts.Update;

public record UserUpdateContract(Guid Id, string? Username, string? Email, Guid UserId);