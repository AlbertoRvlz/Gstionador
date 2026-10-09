namespace GestionTareas.Domain.Entities;

public sealed class Integrante
{
    public Guid Id { get; }
    public string Nombre { get; }

    public Integrante(Guid id, string nombre)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("El identificador es obligatorio.", nameof(id));
        ArgumentException.ThrowIfNullOrWhiteSpace(nombre);
        Id = id;
        Nombre = nombre.Trim();
    }
}
