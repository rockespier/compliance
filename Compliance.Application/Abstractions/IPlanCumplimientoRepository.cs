using Compliance.Domain.Entities;

namespace Compliance.Application.Abstractions;

public interface IPlanCumplimientoRepository
{
    Task<RegistroDeCumplimiento?> ObtenerRegistroPorPlanIdAsync(
        Guid planId,
        CancellationToken cancellationToken = default);

    Task<BaseLegal?> ObtenerBaseLegalPorIdAsync(
        Guid baseLegalId,
        CancellationToken cancellationToken = default);

    Task<Proyecto?> ObtenerProyectoPorIdAsync(
        Guid proyectoId,
        CancellationToken cancellationToken = default);

    Task AgregarPlanAsync(
        PlanDeCumplimiento plan,
        CancellationToken cancellationToken = default);

    Task GuardarCambiosAsync(CancellationToken cancellationToken = default);
}