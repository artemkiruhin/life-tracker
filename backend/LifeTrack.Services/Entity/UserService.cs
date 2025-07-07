using LifeTrack.Core.Interfaces;
using LifeTrack.Core.Interfaces.Services.Entity;
using LifeTrack.Core.Interfaces.Services.Mappers;
using LifeTrack.Core.Interfaces.Services.Security;
using LifeTrack.Core.Models.Contracts;
using LifeTrack.Core.Models.Contracts.Create;
using LifeTrack.Core.Models.Contracts.Specific;
using LifeTrack.Core.Models.Contracts.Update;
using LifeTrack.Core.Models.DTOs;
using LifeTrack.Core.Models.Entities;

namespace LifeTrack.Services.Entity;

public class UserService : IUserService
{
    private readonly IUnitOfWork _database;
    private readonly IHashingService _hasher;
    private readonly IUserMapper _mapper;

    public UserService(IUnitOfWork database, IHashingService hasher, IUserMapper mapper)
    {
        _database = database;
        _hasher = hasher;
        _mapper = mapper;
    }
    
    public async Task<Result<UserDTO>> GetUser(Guid userId, CancellationToken ct)
    {
        try
        {
            var user = await _database.UserRepository.GetByIdAsync(userId, ct);
            if (user == null) return Result<UserDTO>.Failure("User not found");
            var dto = _mapper.Map(user);
            return Result<UserDTO>.Success(dto);
        }
        catch (Exception e)
        {
            return Result<UserDTO>.Failure(e.Message);
        }
    }

    public async Task<Result<UserDTO>> Create(UserCreateContract request, CancellationToken ct)
    {
        await _database.BeginTransactionAsync(ct);
        try
        {
            var userByUsername = await _database.UserRepository.GetByUsernameAsync(request.Username, ct);
            if (userByUsername !=  null) return Result<UserDTO>.Failure("User exists");
            
            var userByEmail = await _database.UserRepository.GetByEmailAsync(request.Email, ct);
            if (userByEmail != null) return Result<UserDTO>.Failure("Email already exists");
            
            var passwordHash = _hasher.HashData(request.Password);
            var generatedSecurityPin = GenerateSecurityPin();
            
            var newUser = UserEntity.Create(request.Username,  passwordHash, request.Email, generatedSecurityPin);
            
            var result = await _database.UserRepository.AddAsync(newUser, ct);
            await _database.SaveChangesAsync(ct);
            await _database.CommitTransactionAsync(ct);

            var dto = _mapper.Map(result);
            return Result<UserDTO>.Success(dto);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<UserDTO>.Failure(e.Message);
        }
    }

    private string GenerateSecurityPin()
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

    public async Task<Result<UserDTO>> Update(UserUpdateContract request, CancellationToken ct)
    {
        await _database.BeginTransactionAsync(ct);
        try
        {
            if (request.Id !=  request.UserId) return Result<UserDTO>.Failure("User not found");
            
            var sender = await _database.UserRepository.GetByIdAsync(request.UserId, ct);
            if (sender == null) return Result<UserDTO>.Failure("User not found");
            
            var user = await _database.UserRepository.GetByIdAsync(request.Id, ct);
            if (user == null) return Result<UserDTO>.Failure("User not found");

            if (!string.IsNullOrEmpty(request.Username) && request.Username != user.Username) 
                user.Username = request.Username;
            if (!string.IsNullOrEmpty(request.Email) && request.Email != user.Email) 
                user.Email = request.Email;
            
            var result = _database.UserRepository.Update(user);
            if (result ==  null) return Result<UserDTO>.Failure("User not found");
            await _database.SaveChangesAsync(ct);
            await _database.CommitTransactionAsync(ct);
            
            var dto = _mapper.Map(result);
            return Result<UserDTO>.Success(dto);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<UserDTO>.Failure(e.Message);
        }
    }

    public async Task<Result<UserDTO>> Delete(Guid userId, CancellationToken ct)
    {
        await _database.BeginTransactionAsync(ct);
        try
        {
            var user = await _database.UserRepository.GetByIdAsync(userId, ct);
            if (user == null) return Result<UserDTO>.Failure("User not found");
            
            var result = _database.UserRepository.Delete(user);
            var dto = _mapper.Map(result);
            return Result<UserDTO>.Success(dto);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<UserDTO>.Failure(e.Message);
        }
    }

    public async Task<Result<UserDTO>> ResetPassword(ResetPasswordContract request, CancellationToken ct)
    {
        await _database.BeginTransactionAsync(ct);
        try
        {
            var user = await _database.UserRepository.GetByIdAsync(request.UserId, ct);
            if (user == null) return Result<UserDTO>.Failure("User not found");

            var newPasswordHash = _hasher.HashData(request.NewPassword);

            if (request.OldPassword == request.NewPassword) return Result<UserDTO>.Failure("New password must be different from old password");
            if (request.NewPassword != request.ConfirmNewPassword) return Result<UserDTO>.Failure("New password and confirmation do not match");

            user.PasswordHash = newPasswordHash;

            var result = _database.UserRepository.Update(user);
            await _database.SaveChangesAsync(ct);
            await _database.CommitTransactionAsync(ct);

            var dto = _mapper.Map(result);
            return Result<UserDTO>.Success(dto);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<UserDTO>.Failure(e.Message);
        }
    }

    public async Task<Result<UserDTO>> ResetPassword(ResetPasswordContractByPin request, CancellationToken ct)
    {
        await _database.BeginTransactionAsync(ct);
        try
        {
            var user = await _database.UserRepository.GetByIdAsync(request.UserId, ct);
            if (user == null) return Result<UserDTO>.Failure("User not found");

            var newPasswordHash = _hasher.HashData(request.NewPassword);

            if (request.Pin != user.SecurityPin) return Result<UserDTO>.Failure("Invalid security PIN");
            
            if (user.PasswordHash == newPasswordHash) return Result<UserDTO>.Failure("New password must be different from old password");
            if (request.NewPassword != request.ConfirmNewPassword) return Result<UserDTO>.Failure("New password and confirmation do not match");

            user.PasswordHash = newPasswordHash;

            var result = _database.UserRepository.Update(user);
            await _database.SaveChangesAsync(ct);
            await _database.CommitTransactionAsync(ct);

            var dto = _mapper.Map(result);
            return Result<UserDTO>.Success(dto);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<UserDTO>.Failure(e.Message);
        }
    }
}