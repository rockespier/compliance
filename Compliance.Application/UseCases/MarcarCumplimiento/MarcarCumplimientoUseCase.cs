using Compliance.Application.Abstractions;
using Compliance.Domain.Entities;

namespace Compliance.Application.UseCases;

public sealed class MarcarCumplimientoUseCase(IPlanCumplimientoRepository repository)
{
    public async Task ExecuteAsync(
        Guid planId,
        MarcarCumplimientoDto dto,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);

        if (dto.PlanId == Guid.Empty)
            throw new ArgumentException("El ID del plan es obligatorio.", nameof(dto));

        var registro = await repository.ObtenerRegistroPorPlanIdAsync(dto.PlanId, cancellationToken);

        if (registro is null)
            throw new KeyNotFoundException($"No existe un registro para el plan con ID '{dto.PlanId}'.");

        var evidencia = new Evidencia(
            dto.EvidenciaIdentificador,
            dto.EvidenciaDescripcion,
            dto.FechaAdjunto);

        registro.AdjuntarEvidencia(evidencia);
        registro.MarcarComoCumplido(dto.FechaCumplimiento);

        await repository.GuardarCambiosAsync(cancellationToken);
    }
}