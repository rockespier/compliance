using Compliance.Domain.Entities;

namespace Compliance.Domain.Interfaces;

public interface IBaseLegalRepository
{
    Task<BaseLegal?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<BaseLegal?> ObtenerPorCodigoAsync(string codigo, CancellationToken cancellationToken = default);
    Task<IEnumerable<BaseLegal>> ObtenerTodasAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<BaseLegal>> ObtenerActivasAsync(CancellationToken cancellationToken = default);
    Task AgregarAsync(BaseLegal baseLegal, CancellationToken cancellationToken = default);
    Task ActualizarAsync(BaseLegal baseLegal, CancellationToken cancellationToken = default);
}