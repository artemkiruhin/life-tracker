using LifeTrack.Core.Models.Contracts.Create;
using LifeTrack.Core.Models.Contracts.Specific;
using LifeTrack.Core.Models.Contracts.Update;
using LifeTrack.Core.Models.DTOs;

namespace LifeTrack.Core.Interfaces.Services.Entity;

public interface IUserService
{
    Task<UserDTO> GetUser(Guid userId, CancellationToken ct);
    Task<UserDTO> Create (UserCreateContract request, CancellationToken ct);
    Task<UserDTO> Update (UserUpdateContract request, CancellationToken ct);
    Task<UserDTO> Delete (Guid userId, CancellationToken ct);
    Task<UserDTO> ResetPassword (ResetPasswordContract request, CancellationToken ct);
    Task<UserDTO> ResetPassword (ResetPasswordContractByPin request, CancellationToken ct);
}