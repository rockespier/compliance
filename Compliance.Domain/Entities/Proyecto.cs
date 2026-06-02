namespace Compliance.Domain.Entities;

public sealed class Proyecto
{
    public Guid Id { get; private set; }
    public string Nombre { get; private set; }
    public string Ubicacion { get; private set; }  // ej. zona de sierra
    public string Fase { get; private set; }

    public Proyecto(string nombre, string ubicacion, string fase)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre del proyecto es obligatorio.", nameof(nombre));
        if (string.IsNullOrWhiteSpace(ubicacion))
            throw new ArgumentException("La ubicación es obligatoria.", nameof(ubicacion));
        if (string.IsNullOrWhiteSpace(fase))
            throw new ArgumentException("La fase es obligatoria.", nameof(fase));

        Id = Guid.NewGuid();
        Nombre = nombre;
        Ubicacion = ubicacion;
        Fase = fase;
    }

    public void AvanzarFase(string nuevaFase)
    {
        if (string.IsNullOrWhiteSpace(nuevaFase))
            throw new ArgumentException("La nueva fase no puede estar vacía.", nameof(nuevaFase));

        Fase = nuevaFase;
    }
}