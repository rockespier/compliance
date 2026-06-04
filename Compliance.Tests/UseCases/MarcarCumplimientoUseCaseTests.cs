using Compliance.Application.Abstractions;
using Compliance.Application.UseCases;
using Compliance.Domain.Entities;
using Compliance.Domain.Enums;
using NSubstitute;
using Xunit;

namespace Compliance.Tests.UseCases;

/// <summary>
/// Conjunto de pruebas unitarias para el caso de uso
/// <c>MarcarCumplimientoUseCase</c>.
///
/// Estas pruebas verifican el comportamiento esperado al intentar
/// marcar como cumplido un registro de cumplimiento, incluyendo la
/// validación de evidencias y la persistencia mediante el repositorio.
/// </summary>
public sealed class MarcarCumplimientoUseCaseTests
{
    /// <summary>
    /// Verifica que cuando existe un <c>RegistroDeCumplimiento</c> asociado
    /// al plan, el caso de uso añade la evidencia, marca el registro como
    /// cumplido y solicita guardar los cambios al repositorio.
    /// </summary>
    [Fact]
    public async Task ExecuteAsyncWhenRegistroExisteAgregaEvidenciaYGuardaCambios()
    {
        var repository = Substitute.For<IPlanCumplimientoRepository>();
        var planId = Guid.NewGuid();
        var registro = CrearRegistro(planId);
        var dto = new MarcarCumplimientoDto(
            planId,
            "DOC-001",
            "Informe de verificacion",
            new DateOnly(2026, 6, 1),
            new DateOnly(2026, 6, 2));

        repository.ObtenerRegistroPorPlanIdAsync(planId, Arg.Any<CancellationToken>())
            .Returns(registro);

        var useCase = new MarcarCumplimientoUseCase(repository);

        await useCase.ExecuteAsync(planId, dto);

        Assert.Equal(EstadoCumplimiento.Cumplido, registro.Estado);
        Assert.Equal(dto.FechaCumplimiento, registro.FechaCumplimiento);
        Assert.Single(registro.Evidencias);
        Assert.Equal("DOC-001", registro.Evidencias[0].Identificador);
        await repository.Received(1).GuardarCambiosAsync(Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Verifica que cuando no existe un <c>RegistroDeCumplimiento</c> para
    /// el identificador de plan proporcionado, el caso de uso lanza
    /// una <see cref="KeyNotFoundException"/>.
    /// </summary>
    [Fact]
    public async Task ExecuteAsyncWhenNoExisteRegistroLanzaKeyNotFoundException()
    {
        var repository = Substitute.For<IPlanCumplimientoRepository>();
        var planId = Guid.NewGuid();
        var dto = new MarcarCumplimientoDto(
            planId,
            "DOC-001",
            "Informe de verificacion",
            new DateOnly(2026, 6, 1),
            new DateOnly(2026, 6, 2));

        repository.ObtenerRegistroPorPlanIdAsync(planId, Arg.Any<CancellationToken>())
            .Returns((RegistroDeCumplimiento?)null);

        var useCase = new MarcarCumplimientoUseCase(repository);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => useCase.ExecuteAsync(planId, dto));

        Assert.Single(repository.ReceivedCalls());
    }

    /// <summary>
    /// Crea un <c>RegistroDeCumplimiento</c> válido asociado a un plan.
    /// Este helper construye las dependencias de dominio necesarias
    /// (<c>BaseLegal</c>, <c>Proyecto</c>, <c>PlanDeCumplimiento</c>) y
    /// devuelve la instancia de registro lista para ser utilizada en pruebas.
    /// </summary>
    /// <param name="planId">Identificador que se usa para la base legal del plan.</param>
    /// <returns>Instancia de <c>RegistroDeCumplimiento</c>.</returns>
    private static RegistroDeCumplimiento CrearRegistro(Guid planId)
    {
        var baseLegal = new BaseLegal(
            planId,
            "DL-123",
            "Descripcion de base legal",
            "MINAM",
            "Ley");
        var proyecto = new Proyecto("Proyecto A", "Lima", "Ejecucion");
        var plan = new PlanDeCumplimiento(baseLegal, proyecto, "Responsable", new DateOnly(2026, 6, 30));

        return new RegistroDeCumplimiento(plan);
    }
}
