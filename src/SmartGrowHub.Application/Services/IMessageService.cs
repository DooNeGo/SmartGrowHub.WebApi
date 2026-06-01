using SmartGrowHub.Domain.Model;
using SmartGrowHub.Domain.Model.Programs;

namespace SmartGrowHub.Application.Services;

public interface IMessageService
{
    IO<Unit> ChangeSchedule(GrowHubModule module, ModuleSchedule schedule, CancellationToken cancellationToken);
}