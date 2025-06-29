namespace LifeTrack.Core.Models.Contracts.Create;

public record KanbanTaskCreateContract(string Title, string? DescriptionMarkdown, bool IsImportant, Guid? TaskCategoryId, Guid UserId);