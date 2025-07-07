using LifeTrack.Core.Interfaces.Services.Mappers;
using LifeTrack.Core.Models.DTOs;
using LifeTrack.Core.Models.Entities;

namespace LifeTrack.Services.Mapper;

public class UserMapper : IUserMapper
{
    public UserDTO Map(UserEntity entity)
    {
        return new UserDTO(
            Id: entity.Id,
            Username: entity.Username,
            Email: entity.Email,
            CreatedAt: entity.CreatedAt,
            UpdatedAt: entity.UpdatedAt
        );
    }
}