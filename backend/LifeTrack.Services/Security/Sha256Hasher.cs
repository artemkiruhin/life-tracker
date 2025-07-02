using System.Security.Cryptography;
using System.Text;
using LifeTrack.Core.Interfaces.Services.Security;

namespace LifeTrack.Services.Security;

public class Sha256Hasher : IHashingService
{
    public string HashData(string message) =>
        Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(message)));
}