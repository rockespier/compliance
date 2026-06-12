using Compliance.Domain.Entities;
using Compliance.Domain.Interfaces;
using Compliance.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Compliance.Infrastructure.Repositories;

public class BaseLegalRepository(AppDbContext context) : IBaseLegalRepository
{
    private readonly AppDbContext _context = context;

    public async Task<BaseLegal?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.BasesLegales
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task<BaseLegal?> ObtenerPorCodigoAsync(string codigo, CancellationToken cancellationToken = default)
    {
        return await _context.BasesLegales
            .FirstOrDefaultAsync(b => b.Codigo == codigo, cancellationToken);
    }

    public async Task<IEnumerable<BaseLegal>> ObtenerTodasAsync(CancellationToken cancellationToken = default)
    {
        return await _context.BasesLegales
            .OrderByDescending(b => b.FechaPublicacion)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<BaseLegal>> ObtenerActivasAsync(CancellationToken cancellationToken = default)
    {
        return await _context.BasesLegales
            .Where(b => b.EstaActiva)
            .OrderByDescending(b => b.FechaPublicacion)
            .ToListAsync(cancellationToken);
    }

    public async Task AgregarAsync(BaseLegal baseLegal, CancellationToken cancellationToken = default)
    {
        await _context.BasesLegales.AddAsync(baseLegal, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task ActualizarAsync(BaseLegal baseLegal, CancellationToken cancellationToken = default)
    {
        _context.BasesLegales.Update(baseLegal);
        await _context.SaveChangesAsync(cancellationToken);
    }
}