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

    public Task GuardarCambiosAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}