using GestionTareas.Domain.Enums;

namespace GestionTareas.Domain.Entities;

// Raíz del agregado: todos los cambios pasan por sus reglas de negocio.
public sealed class Tarea
{
    public Guid Id { get; }
    public string Titulo { get; private set; }
    public string Descripcion { get; private set; }
    public Guid? ResponsableId { get; private set; }
    public EstadoTarea Estado { get; private set; } = EstadoTarea.Pendiente;
    public PrioridadTarea Prioridad { get; private set; }

    public Tarea(string titulo, string descripcion, PrioridadTarea prioridad, Guid? responsableId = null)
    {
        Id = Guid.NewGuid();
        Titulo = ValidarTitulo(titulo);
        Descripcion = ValidarDescripcion(descripcion);
        CambiarPrioridad(prioridad);
        AsignarResponsable(responsableId);
    }

    public void ActualizarDetalles(string titulo, string descripcion)
    {
        // Validar ambos valores antes de modificar el agregado.
        var nuevoTitulo = ValidarTitulo(titulo);
        var nuevaDescripcion = ValidarDescripcion(descripcion);
        Titulo = nuevoTitulo;
        Descripcion = nuevaDescripcion;
    }

    public void AsignarResponsable(Guid? responsableId)
    {
        if (responsableId == Guid.Empty)
            throw new ArgumentException("El responsable debe tener un identificador válido.", nameof(responsableId));
        ResponsableId = responsableId;
    }

    public void CambiarEstado(EstadoTarea estado)
    {
        if (!Enum.IsDefined(estado))
            throw new ArgumentOutOfRangeException(nameof(estado), "Estado no válido.");
        Estado = estado;
    }

    public void CambiarPrioridad(PrioridadTarea prioridad)
    {
        if (!Enum.IsDefined(prioridad))
            throw new ArgumentOutOfRangeException(nameof(prioridad), "Prioridad no válida.");
        Prioridad = prioridad;
    }

    private static string ValidarTitulo(string titulo)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(titulo);
        return titulo.Trim();
    }

    private static string ValidarDescripcion(string descripcion)
    {
        ArgumentNullException.ThrowIfNull(descripcion);
        return descripcion.Trim();
    }
}
