namespace GestionTareasApp.Presentation;

public sealed class MenuConsola
{
    public void Ejecutar()
    {
        while (true)
        {
            Console.WriteLine("\n1. Registrar tarea");
            Console.WriteLine("2. Consultar tareas (estado o responsable)");
            Console.WriteLine("3. Modificar tarea (detalles, responsable, estado o prioridad)");
            Console.WriteLine("4. Eliminar tarea");
            Console.WriteLine("0. Salir");
            Console.Write("Opción: ");
            var opcion = Console.ReadLine();
            if (opcion is null or "0")
                return;

            switch (opcion)
            {
                case "1":
                    Console.WriteLine("Pendiente: Agustín — feature/registro-tareas.");
                    break;
                case "2":
                    Console.WriteLine("Pendiente: Joquin — feature/consulta-tareas.");
                    break;
                case "3":
                    Console.WriteLine("Pendiente: Ángel — feature/modificar-tareas.");
                    break;
                case "4":
                    Console.WriteLine("Pendiente: Edwin — feature/eliminar.");
                    break;
                default:
                    Console.WriteLine("Opción no válida. Selecciona del 0 al 4.");
                    break;
            }
        }
    }
}
