using LifeTrack.Core.Interfaces.Repositories;
using LifeTrack.Core.Interfaces.Services.Entity;
using LifeTrack.Core.Interfaces.Services.Security;
using LifeTrack.Core.Models.Contracts;
using LifeTrack.Core.Models.Contracts.Create;

namespace LifeTrack.Services.Security;

public class AuthentificationService : IAuthentificationService
{
    private readonly IUserService _userService;
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private readonly IHashingService _hashingService;

    public AuthentificationService(IUserService userService, IUserRepository userRepository, IJwtService jwtService, IHashingService hashingService)
    {
        _userService = userService;
        _userRepository = userRepository;
        _jwtService = jwtService;
        _hashingService = hashingService;
    }
    
    public async Task<Result<string>> Login(string username, string password, CancellationToken ct)
    {
        try
        {
            var user = await _userRepository.GetByUsernameAsync(username, ct);
            if (user == null) return Result<string>.Failure("Invalid username or password");

            var passwordHash = _hashingService.HashData(password);
            if (user.PasswordHash != passwordHash) return Result<string>.Failure("Invalid password");

            var token = _jwtService.GenerateToken(user.Id);
            return !token.IsSuccess
                ? Result<string>.Failure("Invalid token")
                : Result<string>.Success(token.Data!);
        }
        catch (Exception e)
        {
            return Result<string>.Failure(e.Message);
        }
    }

    public async Task<Result<Guid>> Registration(string username, string password, string email, CancellationToken ct)
    {
        try
        {
            var userByUsername = await _userRepository.GetByUsernameAsync(username, ct);
            if (userByUsername != null) return Result<Guid>.Failure("Invalid username or password");

            var userByEmail = await _userRepository.GetByEmailAsync(email, ct);
            if (userByEmail != null) return Result<Guid>.Failure("Invalid email");

            var userCreationResult = await _userService.Create(new UserCreateContract(username, password, email), ct);
            return !userCreationResult.IsSuccess
                ? Result<Guid>.Failure("Invalid username or password")
                : Result<Guid>.Success(userCreationResult.Data!.Id);
        }
        catch (Exception e)
        {
            return Result<Guid>.Failure(e.Message);
        }
    }
}