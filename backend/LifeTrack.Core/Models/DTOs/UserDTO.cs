namespace LifeTrack.Core.Models.DTOs;

public record UserDTO(Guid Id, string Username, string Email, DateTime CreatedAt, DateTime? UpdatedAt);