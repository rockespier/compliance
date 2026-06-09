using Compliance.Application.Abstractions;
using Compliance.Application.UseCases.PlanCumplimiento;
using Compliance.Domain.Entities;
using NSubstitute;
using Xunit;

namespace Compliance.Tests.UseCases;

public sealed class CrearPlanCumplimientoUseCaseTests
{
    [Fact]
    public async Task ExecuteAsyncWhenRequestIsValidCreatesPlanAndPersistsIt()
    {
        // Arrange
        var repository = Substitute.For<IPlanCumplimientoRepository>();
        var baseLegalId = Guid.NewGuid();
        var proyectoId = Guid.NewGuid();
        var baseLegal = new BaseLegal(baseLegalId, "DL-001", "Base legal de prueba", "MINAM", "Ley");
        var proyecto = new Proyecto("Proyecto A", "Lima", "Ejecucion");
        var dto = new CrearPlanCumplimientoDto(
            baseLegalId,
            proyectoId,
            "Responsable de Prueba",
            new DateTimeOffset(2026, 6, 30, 0, 0, 0, TimeSpan.Zero));

        repository.ObtenerBaseLegalPorIdAsync(baseLegalId, Arg.Any<CancellationToken>())
            .Returns(baseLegal);
        repository.ObtenerProyectoPorIdAsync(proyectoId, Arg.Any<CancellationToken>())
            .Returns(proyecto);

        var useCase = new CrearPlanCumplimientoUseCase(repository);

        // Act
        var result = await useCase.ExecuteAsync(dto);

        // Assert
        Assert.NotEqual(Guid.Empty, result);
        await repository.Received(1)
            .ObtenerBaseLegalPorIdAsync(baseLegalId, Arg.Any<CancellationToken>());
        await repository.Received(1)
            .ObtenerProyectoPorIdAsync(proyectoId, Arg.Any<CancellationToken>());
        await repository.Received(1)
            .AgregarPlanAsync(
                Arg.Is<PlanDeCumplimiento>(plan =>
                    plan.Id == result &&
                    plan.BaseLegal == baseLegal &&
                    plan.Proyecto == proyecto &&
                    plan.Responsable == dto.Responsable &&
                    plan.FechaLimite == DateOnly.FromDateTime(dto.FechaLimite.UtcDateTime)),
                Arg.Any<CancellationToken>());
        await repository.Received(1).GuardarCambiosAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsyncWhenProyectoDoesNotExistThrowsArgumentException()
    {
        // Arrange
        var repository = Substitute.For<IPlanCumplimientoRepository>();
        var baseLegalId = Guid.NewGuid();
        var proyectoId = Guid.NewGuid();
        var baseLegal = new BaseLegal(baseLegalId, "DL-001", "Base legal de prueba", "MINAM", "Ley");
        var dto = new CrearPlanCumplimientoDto(
            baseLegalId,
            proyectoId,
            "Responsable de Prueba",
            new DateTimeOffset(2026, 6, 30, 0, 0, 0, TimeSpan.Zero));

        repository.ObtenerBaseLegalPorIdAsync(baseLegalId, Arg.Any<CancellationToken>())
            .Returns(baseLegal);
        repository.ObtenerProyectoPorIdAsync(proyectoId, Arg.Any<CancellationToken>())
            .Returns((Proyecto?)null);

        var useCase = new CrearPlanCumplimientoUseCase(repository);

        // Act
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => useCase.ExecuteAsync(dto));

        // Assert
        Assert.Equal($"No existe un Proyecto con ID '{proyectoId}'.", exception.Message);
        await repository.Received(1)
            .ObtenerBaseLegalPorIdAsync(baseLegalId, Arg.Any<CancellationToken>());
        await repository.Received(1)
            .ObtenerProyectoPorIdAsync(proyectoId, Arg.Any<CancellationToken>());
        await repository.DidNotReceive()
            .AgregarPlanAsync(Arg.Any<PlanDeCumplimiento>(), Arg.Any<CancellationToken>());
        await repository.DidNotReceive().GuardarCambiosAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsyncWhenBaseLegalDoesNotExistThrowsInvalidOperationException()
    {
        // Arrange
        var repository = Substitute.For<IPlanCumplimientoRepository>();
        var baseLegalId = Guid.NewGuid();
        var proyectoId = Guid.NewGuid();
        var dto = new CrearPlanCumplimientoDto(
            baseLegalId,
            proyectoId,
            "Responsable de Prueba",
            new DateTimeOffset(2026, 6, 30, 0, 0, 0, TimeSpan.Zero));

        repository.ObtenerBaseLegalPorIdAsync(baseLegalId, Arg.Any<CancellationToken>())
            .Returns((BaseLegal?)null);

        var useCase = new CrearPlanCumplimientoUseCase(repository);

        // Act
        await Assert.ThrowsAsync<InvalidOperationException>(() => useCase.ExecuteAsync(dto));

        // Assert
        await repository.Received(1)
            .ObtenerBaseLegalPorIdAsync(baseLegalId, Arg.Any<CancellationToken>());
        await repository.DidNotReceive()
            .ObtenerProyectoPorIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
        await repository.DidNotReceive()
            .AgregarPlanAsync(Arg.Any<PlanDeCumplimiento>(), Arg.Any<CancellationToken>());
        await repository.DidNotReceive().GuardarCambiosAsync(Arg.Any<CancellationToken>());
    }
}
