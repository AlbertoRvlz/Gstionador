# Gestión de tareas

Cascarón de una aplicación de consola en C# y .NET 10, con Arquitectura Limpia y principios de Domain-Driven Design (DDD). Las funcionalidades de usuario quedan pendientes para el trabajo en equipo.

## Ejecutar

Instalar el SDK de .NET 10. Desde la raíz del repositorio:

```bash
dotnet build GestionTareas.slnx
dotnet run --project GestionTareasApp.csproj
```

Abrir `GestionTareas.slnx` en un IDE compatible con .NET 10 y el formato slnx, o abrir la carpeta en VS Code. No se requieren paquetes NuGet externos ni una base de datos.

## Organización

```text
GestionTareas.slnx
GestionTareasApp.csproj         # Consola: presentación y composición
Program.cs
Presentation/
src/
  GestionTareas.Domain/         # Agregado Tarea, Integrante, enums y repositorios
  GestionTareas.Application/    # Contratos de casos de uso y DTOs
  GestionTareas.Infrastructure/ # Adaptadores de almacenamiento en memoria
```

Dependencias: Application → Domain; Infrastructure → Domain; la consola compone los objetos de Application e Infrastructure. Domain no depende de otras capas. Evitar lógica de negocio en el menú y acceso directo desde las pantallas a los repositorios.

`Tarea` es la raíz del agregado. Sus propiedades tienen setters privados; los cambios pasan por `ActualizarDetalles`, `AsignarResponsable`, `CambiarEstado` y `CambiarPrioridad`. `Integrante` se referencia por su identificador. Este proyecto usa los patrones Repository y casos de uso; DDD guía el modelado del dominio.

## Reparto de trabajo

| Integrante | Funcionalidad | Rama | Contrato a implementar |
| --- | --- | --- | --- |
| Agustín | Registrar con título, descripción, prioridad y responsable opcional | `feature/registro-tareas` | `IRegistrarTarea` |
| Joquin | Listar, buscar por ID y filtrar por estado o responsable | `feature/consulta-tareas` | `IConsultarTareas` |
| Ángel | Modificar detalles, asignación, estado y prioridad | `feature/modificar-tareas` | `IModificarTarea` |
| Edwin | Eliminar una tarea y manejar identificadores inexistentes | `feature/eliminar` | `IEliminarTarea` |

Cada integrante crea su implementación en `src/GestionTareas.Application/Tareas/<funcionalidad>/`, una pantalla propia en `Presentation/` y la conecta desde `Program.cs` y su opción de `MenuConsola`. Coordinar los cambios en estos dos archivos compartidos al integrar.

Las implementaciones reciben `ITareaRepository` por constructor. Registro y modificación reciben también `IIntegranteRepository` para comprobar que un responsable asignado exista. La consola debe compartir una sola instancia de cada repositorio durante la ejecución.

## Contratos y reglas comunes

- ID de tarea: `Guid`, generado por el agregado al crearlo.
- Título obligatorio, sin espacios al principio o al final. Descripción admite texto vacío, pero no null.
- Estado inicial: `Pendiente`. Valores: `Pendiente = 1`, `EnProceso = 2`, `Terminada = 3`.
- Prioridad obligatoria: `Baja = 1`, `Media = 2`, `Alta = 3`.
- Responsable opcional: `Guid?`; null significa sin asignación. Usar los integrantes del repositorio, con IDs estables.
- La consulta sin filtros devuelve todas las tareas. Los filtros indicados se combinan con AND; null significa no aplicar ese filtro.
- Consulta por ID y modificación devuelven null cuando no existe la tarea. Eliminación devuelve false cuando no existe y true cuando se elimina.
- Modificación es una actualización completa. Validar toda la solicitud y la existencia del responsable antes de cambiar el agregado, ya que el repositorio en memoria mantiene referencias a los objetos.
- Capturar errores de entrada en la consola y mostrar mensajes claros, sin cerrar la aplicación.
- El almacenamiento en memoria pierde los datos al cerrar la aplicación y está pensado para esta consola de un solo usuario. Las funcionalidades CRUD del usuario todavía no están implementadas; los métodos del repositorio son infraestructura compartida.

## Flujo de Git

Primero integrar este cascarón en `main`. Cada compañero debe actualizar su copia antes de crear su rama:

```bash
git switch main
git pull --ff-only origin main
git switch -c feature/registro-tareas
```

Cambiar la última rama por la asignada en la tabla. Edwin utiliza `feature/eliminar`.

Al terminar, compilar, revisar cambios y subir la rama:

```bash
dotnet build GestionTareas.slnx
git status
git add <archivos-de-tu-funcionalidad>
git commit -m "feat: implementar registro de tareas"
git push -u origin feature/registro-tareas
```

Crear un pull request hacia `main`, describir la funcionalidad y sus comprobaciones. Ajustar la rama y el mensaje al trabajo de cada integrante.

## Criterios de entrega

- Registro: crea una tarea con ID único y estado Pendiente; rechaza título vacío, prioridad inválida y responsable inexistente.
- Consulta: lista vacía sin errores, búsqueda por ID y filtros individuales/combinados.
- Modificación: actualiza detalles, responsable, estado y prioridad; rechaza datos inválidos sin dejar cambios parciales; maneja tarea inexistente.
- Eliminación: elimina una tarea existente, confirma el resultado y maneja ID inexistente o mal escrito.
- Integración: registrar → consultar → modificar/asignar → filtrar → eliminar. Compilación sin errores.

El menú actual muestra mensajes de funcionalidad pendiente. No representa una aplicación CRUD terminada.
