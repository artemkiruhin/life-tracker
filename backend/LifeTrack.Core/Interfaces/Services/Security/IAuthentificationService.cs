using LifeTrack.Core.Models.Contracts;

namespace LifeTrack.Core.Interfaces.Services.Security;

public interface IAuthentificationService
{
    Task<Result<string>> Login(string username, string password, CancellationToken ct);
    Task<Result<Guid>> Registration(string username, string password, CancellationToken ct);
}