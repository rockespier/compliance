using Compliance.Domain.Enums;
using Compliance.Domain.Exceptions;

namespace Compliance.Domain.Entities;

public sealed class RegistroDeCumplimiento
{
    private readonly List<Evidencia> _evidencias = [];

    public Guid Id { get; private set; }
    public PlanDeCumplimiento Plan { get; private set; }
    public EstadoCumplimiento Estado { get; private set; }
    public DateOnly? FechaCumplimiento { get; private set; }
    public IReadOnlyList<Evidencia> Evidencias => _evidencias.AsReadOnly();

    private RegistroDeCumplimiento()
    {
        Plan = null!;
    }

    public RegistroDeCumplimiento(PlanDeCumplimiento plan)
    {
        ArgumentNullException.ThrowIfNull(plan);

        Id = Guid.NewGuid();
        Plan = plan;
        Estado = EstadoCumplimiento.Pendiente;
    }

    public NivelAlerta NivelAlerta
    {
        get
        {
            if (Estado == EstadoCumplimiento.Cumplido)
                return NivelAlerta.SinAlerta;

            int diasRestantes = Plan.FechaLimite.DayNumber - DateOnly.FromDateTime(DateTime.Today).DayNumber;

            return diasRestantes switch
            {
                <= 0 => NivelAlerta.SinAlerta,
                <= 7 => NivelAlerta.Alerta7Dias,
                <= 15 => NivelAlerta.Alerta15Dias,
                <= 30 => NivelAlerta.Alerta30Dias,
                _ => NivelAlerta.SinAlerta
            };
        }
    }

    public void AdjuntarEvidencia(Evidencia evidencia)
    {
        ArgumentNullException.ThrowIfNull(evidencia);

        if (Estado == EstadoCumplimiento.Cumplido)
            throw new DomainException("No se puede adjuntar evidencia a un registro ya cumplido.");

        _evidencias.Add(evidencia);
    }

    public void MarcarComoCumplido(DateOnly fechaCumplimiento)
    {
        if (Estado == EstadoCumplimiento.Cumplido)
            throw new DomainException("El registro ya está marcado como cumplido.");

        if (_evidencias.Count == 0)
            throw new DomainException("No se puede marcar como 'Cumplido' sin adjuntar al menos una evidencia.");

        Estado = EstadoCumplimiento.Cumplido;
        FechaCumplimiento = fechaCumplimiento;
    }

    public void MarcarComoVencido()
    {
        if (Estado == EstadoCumplimiento.Cumplido)
            throw new DomainException("Un registro cumplido no puede marcarse como vencido.");

        Estado = EstadoCumplimiento.Vencido;
    }
}