using LifeTrack.Core.Interfaces.Services.Security;

namespace LifeTrack.Services.Security;

public class UserSecurityService : IUserSecurityService
{
    public string GenerateSecurityPin()
    {
        var random = new Random();
        var randomPin = new int[4];
        var numberResult = 0d;
        for (var i = 0; i < 4; i++)
        {
            randomPin[i] = random.Next(0, 10);
            numberResult += randomPin[i] * (Math.Pow(10, i));
        }

        return numberResult.ToString();
    }
}