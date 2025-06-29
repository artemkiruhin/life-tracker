namespace LifeTrack.Core.Interfaces.Services.Security;

public interface IJwtService
{
    string GenerateToken(Guid userId);
}