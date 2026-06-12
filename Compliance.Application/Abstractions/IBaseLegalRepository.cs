using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance.Application.Abstractions;

public interface IBaseLegalRepository
{
    // Esempio di metodo generico per evitare interfacce vuote.
    // Sostituire/estendere secondo le necessità reali del dominio.
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}
