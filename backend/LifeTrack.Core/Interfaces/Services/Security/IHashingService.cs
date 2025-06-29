namespace LifeTrack.Core.Interfaces.Services.Security;

public interface IHashingService
{
    string HashData(string message);
}