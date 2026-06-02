using Compliance.Domain.Entities;
using Compliance.Domain.Exceptions;

public sealed class PlanDeCumplimiento
{
    public Guid Id { get; private set; }
    public BaseLegal BaseLegal { get; private set; }
    public Proyecto Proyecto { get; private set; }
    public string Responsable { get; private set; }
    public DateOnly FechaLimite { get; private set; }

    private PlanDeCumplimiento()
    {
        BaseLegal = null!;
        Proyecto = null!;
        Responsable = string.Empty;
    }

    public PlanDeCumplimiento(
        BaseLegal baseLegal,
        Proyecto proyecto,
        string responsable,
        DateOnly fechaLimite)
    {
        ArgumentNullException.ThrowIfNull(baseLegal);
        ArgumentNullException.ThrowIfNull(proyecto);

        if (string.IsNullOrWhiteSpace(responsable))
            throw new ArgumentException("El responsable es obligatorio.", nameof(responsable));

        Id = Guid.NewGuid();
        BaseLegal = baseLegal;
        Proyecto = proyecto;
        Responsable = responsable;
        FechaLimite = fechaLimite;
    }

    public void ReasignarResponsable(string nuevoResponsable)
    {
        if (string.IsNullOrWhiteSpace(nuevoResponsable))
            throw new DomainException("El nuevo responsable no puede estar vacío.");

        Responsable = nuevoResponsable;
    }

    public void ProrrogarFechaLimite(DateOnly nuevaFecha, DateOnly hoy)
    {
        if (nuevaFecha <= hoy)
            throw new DomainException("La nueva fecha límite debe ser posterior a hoy.");

        FechaLimite = nuevaFecha;
    }
}