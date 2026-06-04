using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance.Application.UseCases.PlanCumplimiento;
public record CrearPlanCumplimientoDto(
    Guid BaseLegalId,
    Guid ProyectoId,
    string Responsable,
    DateTimeOffset FechaLimite
);