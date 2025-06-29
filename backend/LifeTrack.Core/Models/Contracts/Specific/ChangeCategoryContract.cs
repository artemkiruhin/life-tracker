namespace LifeTrack.Core.Models.Contracts.Specific;

public record ChangeCategoryContract(Guid TaskId, Guid NewCategoryId, Guid UserId);