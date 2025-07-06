using LifeTrack.Core.Interfaces;
using LifeTrack.Core.Interfaces.Services.Entity;
using LifeTrack.Core.Models.Contracts;
using LifeTrack.Core.Models.Contracts.Create;
using LifeTrack.Core.Models.Contracts.Specific;
using LifeTrack.Core.Models.Contracts.Update;
using LifeTrack.Core.Models.DTOs;
using LifeTrack.Core.Models.Entities;

namespace LifeTrack.Services.Entity;

public class KanbanTaskService : IKanbanTaskService
{
    private readonly IUnitOfWork _database;

    public KanbanTaskService(IUnitOfWork database)
    {
        _database = database;
    }
    
    public async Task<Result<KanbanTaskDTO>> GetTaskById(Guid taskId, Guid userId, CancellationToken ct)
    {
        try
        {
            var task = await _database.KanbanTaskRepository.GetByIdAsync(taskId, ct);
            if (task == null) return Result<KanbanTaskDTO>.Failure("Task not found");
            if (task.UserId !=  userId) return Result<KanbanTaskDTO>.Failure("Access denied");

            var dto = new KanbanTaskDTO(
                Id: task.Id,
                Title: task.Title,
                DescriptionMarkdown: task.DescriptionMarkdown,
                IsImportant: task.IsImportant,
                IsCompleted: task.IsCompleted,
                Category: new KanbanCategoryDTO(
                    Id: task.Category.Id,
                    Name: task.Category.Name,
                    CreatedAt: task.Category.CreatedAt
                ),
                CreatedAt: task.CreatedAt,
                UpdatedAt: task.UpdatedAt
            );
            return Result<KanbanTaskDTO>.Success(dto);
        }
        catch (Exception e)
        {
            return Result<KanbanTaskDTO>.Failure(e.Message);
        }
    }

    public async Task<Result<List<KanbanTaskDTO>>> GetAllCategories(Guid userId, CancellationToken ct)
    {
        try
        {
            var tasks = await _database.KanbanTaskRepository
                .FindRangeAsync(x => x.UserId == userId, ct);

            var dtos = tasks.Select(task => new KanbanTaskDTO(
                Id: task.Id,
                Title: task.Title,
                DescriptionMarkdown: task.DescriptionMarkdown,
                IsImportant: task.IsImportant,
                IsCompleted: task.IsCompleted,
                Category: new KanbanCategoryDTO(
                    Id: task.Category.Id,
                    Name: task.Category.Name,
                    CreatedAt: task.Category.CreatedAt
                ),
                CreatedAt: task.CreatedAt,
                UpdatedAt: task.UpdatedAt
            )).ToList();
            return Result<List<KanbanTaskDTO>>.Success(dtos);
        }
        catch (Exception e)
        {
            return Result<List<KanbanTaskDTO>>.Failure(e.Message);
        }
    }

    public async Task<Result<KanbanTaskDTO>> Create(KanbanTaskCreateContract request, CancellationToken ct)
    {
        await _database.BeginTransactionAsync(ct);
        try
        {
            var newTask = KanbanTaskEntity.Create(request.Title, request.DescriptionMarkdown, request.IsImportant, request.TaskCategoryId, request.UserId);
            
            var result = await _database.KanbanTaskRepository.AddAsync(newTask, ct);
            await _database.SaveChangesAsync(ct);
            await _database.CommitTransactionAsync(ct);
            
            var dto = new KanbanTaskDTO(
                Id: result.Id,
                Title: result.Title,
                DescriptionMarkdown: result.DescriptionMarkdown,
                IsImportant: result.IsImportant,
                IsCompleted: result.IsCompleted,
                Category: new KanbanCategoryDTO(
                    Id: result.Category.Id,
                    Name: result.Category.Name,
                    CreatedAt: result.Category.CreatedAt
                ),
                CreatedAt: result.CreatedAt,
                UpdatedAt: result.UpdatedAt
            );
            return Result<KanbanTaskDTO>.Success(dto);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<KanbanTaskDTO>.Failure(e.Message);
        }
    }

    public async Task<Result<KanbanTaskDTO>> Update(KanbanTaskUpdateContract request, CancellationToken ct)
    {
        await _database.BeginTransactionAsync(ct);
        try
        {
            var taskById = await _database.KanbanTaskRepository.GetByIdAsync(request.Id, ct);
            if (taskById == null) return Result<KanbanTaskDTO>.Failure("Task not found");
            if (taskById.UserId != request.UserId) return Result<KanbanTaskDTO>.Failure("Access denied");
            
            if (!string.IsNullOrEmpty(request.Title)) taskById.Title = request.Title;
            if (!string.IsNullOrEmpty(request.DescriptionMarkdown))
                taskById.DescriptionMarkdown = request.DescriptionMarkdown == ""
                    ? null
                    : request.DescriptionMarkdown;
            if (request.IsImportant.HasValue) taskById.IsImportant = request.IsImportant.Value;
            if (request.TaskCategoryId.HasValue) taskById.TaskCategoryId = request.TaskCategoryId.Value;
            

            var result = _database.KanbanTaskRepository.Update(taskById);
            if (result == null) return Result<KanbanTaskDTO>.Failure("Task not found");
            await _database.SaveChangesAsync(ct);
            await _database.CommitTransactionAsync(ct);
            
            var dto = new KanbanTaskDTO(
                Id: result.Id,
                Title: result.Title,
                DescriptionMarkdown: result.DescriptionMarkdown,
                IsImportant: result.IsImportant,
                IsCompleted: result.IsCompleted,
                Category: new KanbanCategoryDTO(
                    Id: result.Category.Id,
                    Name: result.Category.Name,
                    CreatedAt: result.Category.CreatedAt
                ),
                CreatedAt: result.CreatedAt,
                UpdatedAt: result.UpdatedAt
            );
            return Result<KanbanTaskDTO>.Success(dto);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<KanbanTaskDTO>.Failure(e.Message);
        }
    }

    public async Task<Result<KanbanTaskDTO>> Delete(Guid taskId, Guid userId, CancellationToken ct)
    {
        await _database.BeginTransactionAsync(ct);
        try
        {
            var task = await _database.KanbanTaskRepository.GetByIdAsync(taskId, ct);
            if (task == null) return Result<KanbanTaskDTO>.Failure("Task not found");
            
            if (task.UserId != userId) return Result<KanbanTaskDTO>.Failure("Access denied");
            
            var result = _database.KanbanTaskRepository.Delete(task);
            await _database.SaveChangesAsync(ct);
            await _database.CommitTransactionAsync(ct);
            
            var dto = new KanbanTaskDTO(
                Id: result.Id,
                Title: result.Title,
                DescriptionMarkdown: result.DescriptionMarkdown,
                IsImportant: result.IsImportant,
                IsCompleted: result.IsCompleted,
                Category: new KanbanCategoryDTO(
                    Id: result.Category.Id,
                    Name: result.Category.Name,
                    CreatedAt: result.Category.CreatedAt
                ),
                CreatedAt: result.CreatedAt,
                UpdatedAt: result.UpdatedAt
            );
            return Result<KanbanTaskDTO>.Success(dto);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<KanbanTaskDTO>.Failure(e.Message);
        }
    }

    public async Task<Result<KanbanTaskDTO>> RemoveCategory(RemoveCategoryContract request, CancellationToken ct)
    {
        try
        {
            var taskById = await _database.KanbanTaskRepository.GetByIdAsync(request.TaskId, ct);
            if (taskById == null) return Result<KanbanTaskDTO>.Failure("Task not found");
            if (taskById.UserId != request.UserId) return Result<KanbanTaskDTO>.Failure("Access denied");
            
            taskById.TaskCategoryId = null;
            var result = _database.KanbanTaskRepository.Update(taskById);
            if (result == null) return Result<KanbanTaskDTO>.Failure("Task not found");
            await _database.SaveChangesAsync(ct);
            await _database.CommitTransactionAsync(ct);
            
            var dto = new KanbanTaskDTO(
                Id: result.Id,
                Title: result.Title,
                DescriptionMarkdown: result.DescriptionMarkdown,
                IsImportant: result.IsImportant,
                IsCompleted: result.IsCompleted,
                Category: new KanbanCategoryDTO(
                    Id: result.Category.Id,
                    Name: result.Category.Name,
                    CreatedAt: result.Category.CreatedAt
                ),
                CreatedAt: result.CreatedAt,
                UpdatedAt: result.UpdatedAt
            );
            return Result<KanbanTaskDTO>.Success(dto);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<KanbanTaskDTO>.Failure(e.Message);
        }
    }

    public async Task<Result<KanbanTaskDTO>> ChangeCategory(ChangeCategoryContract request, CancellationToken ct)
    {
        try
        {
            var taskById = await _database.KanbanTaskRepository.GetByIdAsync(request.TaskId, ct);
            if (taskById == null) return Result<KanbanTaskDTO>.Failure("Task not found");
            if (taskById.UserId != request.UserId) return Result<KanbanTaskDTO>.Failure("Access denied");
            
            var categoryById = await _database.KanbanCategoryRepository.GetByIdAsync(request.NewCategoryId, ct);
            if (categoryById == null) return Result<KanbanTaskDTO>.Failure("Category not found");
            if (categoryById.UserId != request.UserId) return Result<KanbanTaskDTO>.Failure("Access denied");
            
            taskById.TaskCategoryId = request.NewCategoryId;
            var result = _database.KanbanTaskRepository.Update(taskById);
            if (result == null) return Result<KanbanTaskDTO>.Failure("Task not found");
            await _database.SaveChangesAsync(ct);
            await _database.CommitTransactionAsync(ct);
            
            var dto = new KanbanTaskDTO(
                Id: result.Id,
                Title: result.Title,
                DescriptionMarkdown: result.DescriptionMarkdown,
                IsImportant: result.IsImportant,
                IsCompleted: result.IsCompleted,
                Category: new KanbanCategoryDTO(
                    Id: result.Category.Id,
                    Name: result.Category.Name,
                    CreatedAt: result.Category.CreatedAt
                ),
                CreatedAt: result.CreatedAt,
                UpdatedAt: result.UpdatedAt
            );
            return Result<KanbanTaskDTO>.Success(dto);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<KanbanTaskDTO>.Failure(e.Message);
        }
    }
}