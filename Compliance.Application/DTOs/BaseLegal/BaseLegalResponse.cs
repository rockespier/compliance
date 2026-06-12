namespace Compliance.Application.DTOs.BaseLegal;

public record BaseLegalResponse(
    Guid Id,
    string Codigo,
    string Denominacion,
    string TipoNorma,
    DateOnly FechaPublicacion,
    string? Descripcion,
    string? UrlDocumento,
    bool EstaActiva,
    DateTime FechaCreacion
);