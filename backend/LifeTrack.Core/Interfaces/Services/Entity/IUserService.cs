using LifeTrack.Core.Models.Contracts;
using LifeTrack.Core.Models.Contracts.Create;
using LifeTrack.Core.Models.Contracts.Specific;
using LifeTrack.Core.Models.Contracts.Update;
using LifeTrack.Core.Models.DTOs;

namespace LifeTrack.Core.Interfaces.Services.Entity;

public interface IUserService
{
    Task<Result<UserDTO>> GetUser(Guid userId, CancellationToken ct);
    Task<Result<UserDTO>> Create (UserCreateContract request, CancellationToken ct);
    Task<Result<UserDTO>> Update (UserUpdateContract request, CancellationToken ct);
    Task<Result<UserDTO>> Delete (Guid userId, CancellationToken ct);
    Task<Result<UserDTO>> ResetPassword (ResetPasswordContract request, CancellationToken ct);
    Task<Result<UserDTO>> ResetPassword (ResetPasswordContractByPin request, CancellationToken ct);
}