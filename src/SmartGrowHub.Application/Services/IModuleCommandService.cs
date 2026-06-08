using SmartGrowHub.Domain.Model;
using SmartGrowHub.Domain.Model.Programs;

namespace SmartGrowHub.Application.Services;

public interface IModuleCommandService
{
    IO<Unit> ChangeSchedule(GrowHubModule module, ModuleSchedule schedule);
}