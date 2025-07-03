using LifeTrack.Core.Interfaces.Repositories;
using LifeTrack.Core.Models.Entities;
using LifeTrack.Infractructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace LifeTrack.Infractructure.Repositories;

public class UserRepository(AppDbContext context) : BaseRepository<UserEntity>(context), IUserRepository
{
    public async Task<UserEntity?> GetByEmailAsync(string email, CancellationToken ct)
    {
        return await DbSet.AsNoTracking().Where(user => user.Email == email).FirstOrDefaultAsync(ct);
    }

    public async Task<UserEntity?> GetByUsernameAsync(string username, CancellationToken ct)
    {
        return await DbSet.AsNoTracking().Where(user => user.Username == username).FirstOrDefaultAsync(ct);
    }
}