using LifeTrack.Core.Models.DTOs;
using LifeTrack.Core.Models.Entities;

namespace LifeTrack.Core.Interfaces.Services.Mappers;

public interface IKanbanTaskMapper
{
    KanbanTaskDTO Map (KanbanTaskEntity entity);
}