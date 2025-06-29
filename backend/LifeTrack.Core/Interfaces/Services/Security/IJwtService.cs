using LifeTrack.Core.Models.Contracts;

namespace LifeTrack.Core.Interfaces.Services.Security;

public interface IJwtService
{
    Result<string> GenerateToken(Guid userId);
}