namespace LifeTrack.Core.Models.Contracts.Update;

public record KanbanTaskUpdateContract(Guid Id, string? Title, string? DescriptionMarkdown, bool? IsImportant, Guid? TaskCategoryId, Guid UserId);