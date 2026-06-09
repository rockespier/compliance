using Compliance.Application.Abstractions;
using Compliance.Domain.Entities;

namespace Compliance.Application.UseCases.PlanCumplimiento;

// Usamos Primary Constructors de C# 12/13 para inyectar el repositorio fácilmente
public class CrearPlanCumplimientoUseCase(IPlanCumplimientoRepository repository)
{
    public async Task<Guid> ExecuteAsync(CrearPlanCumplimientoDto dto, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);

        // 1. Validaciones básicas de negocio
        if (dto.BaseLegalId == Guid.Empty || dto.ProyectoId == Guid.Empty)
            throw new ArgumentException("Los IDs de la Base Legal y del Proyecto son obligatorios.");

        if (string.IsNullOrWhiteSpace(dto.Responsable))
            throw new ArgumentException("El responsable no puede estar vacío.");

        // 2. Cargar Base Legal y Proyecto desde el repositorio
        var baseLegal = await repository.ObtenerBaseLegalPorIdAsync(dto.BaseLegalId, cancellationToken);
        if (baseLegal is null)
            throw new InvalidOperationException($"No existe una Base Legal con ID '{dto.BaseLegalId}'.");

        var proyecto = await repository.ObtenerProyectoPorIdAsync(dto.ProyectoId, cancellationToken);
        if (proyecto is null)
            throw new ArgumentException($"No existe un Proyecto con ID '{dto.ProyectoId}'.");

        // 3. Creación de la Entidad de Dominio
        var nuevoPlan = new PlanDeCumplimiento(
            baseLegal,
            proyecto,
            dto.Responsable,
            DateOnly.FromDateTime(dto.FechaLimite.UtcDateTime));

        // 4. Persistencia a través del puerto (Interfaz)
        await repository.AgregarPlanAsync(nuevoPlan, cancellationToken);
        await repository.GuardarCambiosAsync(cancellationToken);

        // 5. Retornamos el ID recién creado
        return nuevoPlan.Id;
    }
}
