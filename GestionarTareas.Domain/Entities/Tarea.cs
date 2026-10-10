using GestionTareas.Domain.Enums;

namespace GestionTareas.Domain.Entities;

public class Tarea
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int? ResponsableId { get; set; }
    public Integrante? Responsable { get; set; }
    public EstadoTarea Estado { get; set; } = EstadoTarea.Pendiente;
    public PrioridadTarea Prioridad { get; set; } = PrioridadTarea.Media;
}