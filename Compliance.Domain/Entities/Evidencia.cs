namespace Compliance.Domain.Entities;

public sealed class Evidencia
{
    public Guid Id { get; private set; }
    public string Identificador { get; private set; }   // ruta, URL, código de documento
    public string Descripcion { get; private set; }
    public DateOnly FechaAdjunto { get; private set; }

    public Evidencia(string identificador, string descripcion, DateOnly fechaAdjunto)
    {
        if (string.IsNullOrWhiteSpace(identificador))
            throw new ArgumentException("El identificador de evidencia es obligatorio.", nameof(identificador));
        if (string.IsNullOrWhiteSpace(descripcion))
            throw new ArgumentException("La descripción de evidencia es obligatoria.", nameof(descripcion));

        Id = Guid.NewGuid();
        Identificador = identificador;
        Descripcion = descripcion;
        FechaAdjunto = fechaAdjunto;
    }
}