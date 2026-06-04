using Compliance.Application.Abstractions;
using Compliance.Domain.Entities;
using Compliance.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Compliance.Infrastructure.Repositories;

public sealed class PlanCumplimientoRepository(AppDbContext dbContext) : IPlanCumplimientoRepository
{
    public Task<RegistroDeCumplimiento?> ObtenerRegistroPorPlanIdAsync(
        Guid planId,
        CancellationToken cancellationToken = default)
    {
        return dbContext.RegistrosCumplimiento
            .Include(x => x.Evidencias)
            .SingleOrDefaultAsync(
                x => EF.Property<Guid>(x, "PlanId") == planId,
                cancellationToken);
    }

    public Task<BaseLegal?> ObtenerBaseLegalPorIdAsync(
        Guid baseLegalId,
        CancellationToken cancellationToken = default)
    {
        return dbContext.BasesLegales
            .SingleOrDefaultAsync(x => x.Id == baseLegalId, cancellationToken);
    }

    public Task<Proyecto?> ObtenerProyectoPorIdAsync(
        Guid proyectoId,
        CancellationToken cancellationToken = default)
    {
        return dbContext.Proyectos
            .SingleOrDefaultAsync(x => x.Id == proyectoId, cancellationToken);
    }

    public Task AgregarPlanAsync(
        PlanDeCumplimiento plan,
        CancellationToken cancellationToken = default)
    {
        return dbContext.PlanesCumplimiento.AddAsync(plan, cancellationToken).AsTask();
    }

    public Task GuardarCambiosAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}