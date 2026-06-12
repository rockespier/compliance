using Compliance.Application.DTOs.BaseLegal;
using Compliance.Domain.Entities;
using Compliance.Domain.Interfaces;

namespace Compliance.Application.UseCases.BaseLegal;

public class RegistrarBaseLegalUseCase(IBaseLegalRepository baseLegalRepository)
{
    private readonly IBaseLegalRepository _baseLegalRepository = baseLegalRepository;

    public async Task<BaseLegalResponse> ExecuteAsync(
        RegistrarBaseLegalRequest request, 
        CancellationToken cancellationToken = default)
    {
        // Validar que no exista una base legal con el mismo código
        Domain.Entities.BaseLegal? baseLegalExistente = await _baseLegalRepository
            .ObtenerPorCodigoAsync(request.Codigo, cancellationToken);

        if (baseLegalExistente is not null)
        {
            throw new InvalidOperationException(
                $"Ya existe una base legal con el código '{request.Codigo}'.");
        }

        // Crear la entidad de dominio usando el constructor
        var nuevaBaseLegal = new Domain.Entities.BaseLegal(
            Guid.NewGuid(),
            request.Codigo,
            request.Descripcion ?? string.Empty,
            request.TipoNorma,
            request.TipoNorma
        );

        // Persistir en el repositorio
        await _baseLegalRepository.AgregarAsync(nuevaBaseLegal, cancellationToken);

        // Retornar el DTO de respuesta
        return MapearAResponse(nuevaBaseLegal);
    }

    private static BaseLegalResponse MapearAResponse(Domain.Entities.BaseLegal baseLegal)
    {
        return new BaseLegalResponse(
            baseLegal.Id,
            baseLegal.Codigo,
            baseLegal.Descripcion,
            baseLegal.Tipo,
            DateOnly.FromDateTime(baseLegal.FechaPublicacion),
            baseLegal.Descripcion,
            null,
            baseLegal.EstaActiva,
            baseLegal.FechaPublicacion
        );
    }
}