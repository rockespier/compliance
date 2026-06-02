namespace Compliance.Domain.Entities;

public sealed class BaseLegal
{
    public Guid Id { get; private set; }
    public string Codigo { get; private set; }
    public string Descripcion { get; private set; }
    public string Organismo { get; private set; }// OEFA, MEM, MINEM, Tributario, Laboral
    public string Tipo { get; private set; }

    public BaseLegal(Guid id, string codigo, string descripcion, string organismo, string tipo)
    {
        if (string.IsNullOrWhiteSpace(codigo))
            throw new ArgumentException("El código de la base legal es obligatorio.", nameof(codigo));
        if (string.IsNullOrWhiteSpace(descripcion))
            throw new ArgumentException("La descripción es obligatoria.", nameof(descripcion));
        if (string.IsNullOrWhiteSpace(organismo))
            throw new ArgumentException("El organismo es obligatorio.", nameof(organismo));

        Id = Guid.NewGuid();
        Codigo = codigo;
        Descripcion = descripcion;
        Organismo = organismo;
        Tipo = tipo;
    }
}