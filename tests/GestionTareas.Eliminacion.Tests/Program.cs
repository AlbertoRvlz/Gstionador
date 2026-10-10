using System.Text;
using GestionTareas.Application.Tareas.Eliminar;
using GestionTareas.Domain.Entities;
using GestionTareas.Domain.Enums;
using GestionTareas.Infrastructure.Repositories;
using GestionTareasApp.Presentation;

Console.OutputEncoding = Encoding.UTF8;

// Pruebas ejecutables sin paquetes externos. Un fallo devuelve código de salida 1.
var pruebas = new (string Nombre, Action Ejecutar)[]
{
    ("Elimina solo la tarea indicada", EliminaSoloLaTareaIndicada),
    ("La segunda eliminación informa que la tarea no existe", EliminarDosVeces),
    ("Un ID inexistente conserva las tareas", IdInexistente),
    ("Rechaza el GUID vacío en el caso de uso", RechazaGuidVacio),
    ("Una entrada inválida no elimina tareas", EntradaInvalida),
    ("Cancelar conserva la tarea", CancelarConservaLaTarea),
    ("Confirmar elimina la tarea y muestra el resultado", ConfirmarEliminaLaTarea),
    ("La pantalla informa cuando la tarea no existe", PantallaIdInexistente),
    ("El fin de la entrada cancela la eliminación", FinDeEntrada)
};

var fallos = 0;
foreach (var prueba in pruebas)
{
    try
    {
        prueba.Ejecutar();
        Console.WriteLine($"OK: {prueba.Nombre}");
    }
    catch (Exception ex)
    {
        fallos++;
        Console.WriteLine($"ERROR: {prueba.Nombre}: {ex.Message}");
    }
}

Console.WriteLine($"\n{pruebas.Length - fallos}/{pruebas.Length} pruebas correctas.");
return fallos == 0 ? 0 : 1;

static (TareaRepositoryEnMemoria Repositorio, Tarea Tarea) Preparar()
{
    var repositorio = new TareaRepositoryEnMemoria();
    var tarea = new Tarea("Preparar entrega", "Revisar el proyecto", PrioridadTarea.Media);
    repositorio.Agregar(tarea);
    return (repositorio, tarea);
}

static void Verificar(bool condicion, string mensaje)
{
    if (!condicion)
        throw new InvalidOperationException(mensaje);
}

static string MostrarPantalla(TareaRepositoryEnMemoria repositorio, string entrada)
{
    using var lector = new StringReader(entrada);
    using var salida = new StringWriter();
    new PantallaEliminarTarea(new EliminarTarea(repositorio), lector, salida).Ejecutar();
    return salida.ToString();
}

static void EliminaSoloLaTareaIndicada()
{
    var (repositorio, tarea) = Preparar();
    var otra = new Tarea("Otra tarea", "", PrioridadTarea.Alta);
    repositorio.Agregar(otra);
    Verificar(new EliminarTarea(repositorio).Ejecutar(tarea.Id), "Debe confirmar la eliminación.");
    Verificar(repositorio.ObtenerPorId(tarea.Id) is null, "La tarea sigue registrada.");
    Verificar(repositorio.Consultar().Count == 1 && repositorio.ObtenerPorId(otra.Id) is not null,
        "Debe conservar la otra tarea.");
}

static void EliminarDosVeces()
{
    var (repositorio, tarea) = Preparar();
    var casoDeUso = new EliminarTarea(repositorio);
    Verificar(casoDeUso.Ejecutar(tarea.Id), "La primera eliminación debe funcionar.");
    Verificar(!casoDeUso.Ejecutar(tarea.Id), "La segunda debe devolver false.");
}

static void IdInexistente()
{
    var (repositorio, tarea) = Preparar();
    Verificar(!new EliminarTarea(repositorio).Ejecutar(Guid.NewGuid()), "Debe devolver false.");
    Verificar(repositorio.ObtenerPorId(tarea.Id) is not null, "Se eliminó una tarea distinta.");
}

static void RechazaGuidVacio()
{
    var (repositorio, tarea) = Preparar();
    var rechazado = false;
    try
    {
        new EliminarTarea(repositorio).Ejecutar(Guid.Empty);
    }
    catch (ArgumentException)
    {
        rechazado = true;
    }
    Verificar(rechazado, "Debe rechazar un GUID vacío.");
    Verificar(repositorio.ObtenerPorId(tarea.Id) is not null, "Debe conservar las tareas.");
}

static void EntradaInvalida()
{
    foreach (var entrada in new[] { "abc\ns\n", $"{Guid.Empty}\ns\n" })
    {
        var (repositorio, tarea) = Preparar();
        var salida = MostrarPantalla(repositorio, entrada);
        Verificar(salida.Contains("Identificador no válido"), "Debe informar el error de entrada.");
        Verificar(repositorio.ObtenerPorId(tarea.Id) is not null, "La entrada inválida eliminó una tarea.");
    }
}

static void CancelarConservaLaTarea()
{
    foreach (var respuesta in new[] { "n", "", "si", "otra respuesta" })
    {
        var (repositorio, tarea) = Preparar();
        var salida = MostrarPantalla(repositorio, $"{tarea.Id}\n{respuesta}\n");
        Verificar(salida.Contains("Eliminación cancelada"), "Debe confirmar la cancelación.");
        Verificar(repositorio.ObtenerPorId(tarea.Id) is not null, "Se eliminó sin confirmar con s.");
    }
    var (otroRepositorio, otraTarea) = Preparar();
    var salidaVacia = MostrarPantalla(otroRepositorio, "\n");
    Verificar(salidaVacia.Contains("Eliminación cancelada"), "Enter debe cancelar.");
    Verificar(otroRepositorio.ObtenerPorId(otraTarea.Id) is not null, "Enter eliminó una tarea.");
}

static void ConfirmarEliminaLaTarea()
{
    foreach (var respuesta in new[] { "s", " S " })
    {
        var (repositorio, tarea) = Preparar();
        var salida = MostrarPantalla(repositorio, $" {tarea.Id} \n{respuesta}\n");
        Verificar(salida.Contains("Tarea eliminada correctamente"), "Falta el mensaje de éxito.");
        Verificar(repositorio.ObtenerPorId(tarea.Id) is null, "La tarea sigue registrada.");
    }
}

static void PantallaIdInexistente()
{
    var (repositorio, tarea) = Preparar();
    var salida = MostrarPantalla(repositorio, $"{Guid.NewGuid()}\ns\n");
    Verificar(salida.Contains("No existe una tarea"), "Debe explicar que el ID no existe.");
    Verificar(repositorio.ObtenerPorId(tarea.Id) is not null, "Debe conservar la tarea existente.");
}

static void FinDeEntrada()
{
    foreach (var confirmar in new[] { false, true })
    {
        var (repositorio, tarea) = Preparar();
        var salida = MostrarPantalla(repositorio, confirmar ? $"{tarea.Id}\n" : "");
        Verificar(salida.Contains("Eliminación cancelada"), "El fin de entrada debe cancelar.");
        Verificar(repositorio.ObtenerPorId(tarea.Id) is not null, "El fin de entrada eliminó una tarea.");
    }
}
