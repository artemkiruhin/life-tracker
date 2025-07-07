using LifeTrack.Core.Interfaces;
using LifeTrack.Core.Interfaces.Services.Entity;
using LifeTrack.Core.Interfaces.Services.Mappers;
using LifeTrack.Core.Models.Contracts;
using LifeTrack.Core.Models.Contracts.Create;
using LifeTrack.Core.Models.Contracts.Update;
using LifeTrack.Core.Models.DTOs;
using LifeTrack.Core.Models.Entities;

namespace LifeTrack.Services.Entity;

public class KanbanCategoryService :  IKanbanCategoryService
{
    private readonly IUnitOfWork _database;
    private readonly IKanbanCategoryMapper _mapper;

    public KanbanCategoryService(IUnitOfWork database, IKanbanCategoryMapper mapper)
    {
        _database = database;
        _mapper = mapper;
    }
    
    public async Task<Result<KanbanCategoryDTO>> GetCategoryById(Guid categoryId, Guid userId, CancellationToken ct)
    {
        try
        {
            var category = await _database.KanbanCategoryRepository.GetByIdAsync(categoryId, ct);
            if (category == null) return Result<KanbanCategoryDTO>.Failure("Category not found");
            if (category.UserId !=  userId) return Result<KanbanCategoryDTO>.Failure("Access denied");
            
            var dto = _mapper.Map(category);
            return Result<KanbanCategoryDTO>.Success(dto);
        }
        catch (Exception e)
        {
            return Result<KanbanCategoryDTO>.Failure(e.Message);
        }
    }

    public async Task<Result<List<KanbanCategoryDTO>>> GetAllCategories(Guid userId, CancellationToken ct)
    {
        try
        {
            var categories = await _database.KanbanCategoryRepository
                .FindRangeAsync(x => x.UserId == userId, ct);
            var dtos = categories.Select(_mapper.Map).ToList();
            return Result<List<KanbanCategoryDTO>>.Success(dtos);
        }
        catch (Exception e)
        {
            return Result<List<KanbanCategoryDTO>>.Failure(e.Message);
        }
    }

    public async Task<Result<KanbanCategoryDTO>> Create(KanbanCategoryCreateContract request, CancellationToken ct)
    {
        await _database.BeginTransactionAsync(ct);
        try
        {
            var categoryByName = await _database.KanbanCategoryRepository.FindAsync(x => x.Name == request.Name,ct);
            if (categoryByName != null) return Result<KanbanCategoryDTO>.Failure("Category with this name already exists");

            var newCategory = KanbanTaskCategoryEntity.Create(request.Name, request.UserId);
            var result =  await _database.KanbanCategoryRepository.AddAsync(newCategory, ct);
            await _database.SaveChangesAsync(ct);
            await _database.CommitTransactionAsync(ct);

            var dto = _mapper.Map(result);
            return Result<KanbanCategoryDTO>.Success(dto);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<KanbanCategoryDTO>.Failure(e.Message);
        }
    }
    
    public async Task<Result<KanbanCategoryDTO>> Update(KanbanCategoryUpdateContract request, CancellationToken ct)
    {
        await _database.BeginTransactionAsync(ct);
        try
        {
            var category = await _database.KanbanCategoryRepository.GetByIdAsync(request.Id, ct);
            if (category == null) return Result<KanbanCategoryDTO>.Failure("Category not found");

            if (category.UserId != request.UserId) return Result<KanbanCategoryDTO>.Failure("Access denied");
            
            if (!string.IsNullOrEmpty(request.Name))
            {
                var categoryByName = await _database.KanbanCategoryRepository.FindAsync(x => x.Name == request.Name,ct);
                if (categoryByName != null) return Result<KanbanCategoryDTO>.Failure("Category with this name already exists");
                
                category.Name = request.Name;
            }
            
            var result =  _database.KanbanCategoryRepository.Update(category);
            await _database.SaveChangesAsync(ct);
            await _database.CommitTransactionAsync(ct);

            var dto = _mapper.Map(result);
            return Result<KanbanCategoryDTO>.Success(dto);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<KanbanCategoryDTO>.Failure(e.Message);
        }
    }

    public async Task<Result<KanbanCategoryDTO>> Delete(Guid categoryId, Guid userId, CancellationToken ct)
    {
        await _database.BeginTransactionAsync(ct);
        try
        {
            var category = await _database.KanbanCategoryRepository.GetByIdAsync(categoryId, ct);
            if (category == null) return Result<KanbanCategoryDTO>.Failure("Category not found");
            
            if (category.UserId != userId) return Result<KanbanCategoryDTO>.Failure("Access denied");
            
            var result = _database.KanbanCategoryRepository.Delete(category);
            await _database.SaveChangesAsync(ct);
            await _database.CommitTransactionAsync(ct);
            
            var dto = _mapper.Map(result);
            return Result<KanbanCategoryDTO>.Success(dto);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<KanbanCategoryDTO>.Failure(e.Message);
        }
    }
}