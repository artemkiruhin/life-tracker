using LifeTrack.Core.Interfaces.Repositories.Base;
using LifeTrack.Core.Models.Entities;

namespace LifeTrack.Core.Interfaces.Repositories;

public interface IUserRepository: IRepository<UserEntity>
{
    Task<UserEntity?> GetByEmailAsync(string email, CancellationToken ct);
    Task<UserEntity?> GetByUsernameAsync(string username, CancellationToken ct);
}