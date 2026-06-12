namespace Compliance.Application.DTOs.BaseLegal;

public record RegistrarBaseLegalRequest(
    string Codigo,
    string Denominacion,
    string TipoNorma,
    DateOnly FechaPublicacion,
    string? Descripcion,
    string? UrlDocumento
);