namespace Compliance.Application.UseCases;

public sealed record MarcarCumplimientoDto(
    Guid PlanId,
    string EvidenciaIdentificador,
    string EvidenciaDescripcion,
    DateOnly FechaAdjunto,
    DateOnly FechaCumplimiento);