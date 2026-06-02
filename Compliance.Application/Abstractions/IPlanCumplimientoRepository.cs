using Compliance.Domain.Entities;

namespace Compliance.Application.Abstractions;

public interface IPlanCumplimientoRepository
{
    Task<RegistroDeCumplimiento?> ObtenerRegistroPorPlanIdAsync(
        Guid planId,
        CancellationToken cancellationToken = default);

    Task GuardarCambiosAsync(CancellationToken cancellationToken = default);
}