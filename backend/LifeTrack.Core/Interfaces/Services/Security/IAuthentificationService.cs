namespace LifeTrack.Core.Interfaces.Services.Security;

public interface IAuthentificationService
{
    Task<string> Login(string username, string password, CancellationToken ct);
    Task<Guid> Registration(string username, string password, CancellationToken ct);
}