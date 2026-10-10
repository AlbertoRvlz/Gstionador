# Gestión de tareas

Aplicación de consola en C# y .NET 10, con Arquitectura Limpia y principios de Domain-Driven Design (DDD). La eliminación de tareas está implementada; registro, consulta y modificación quedan pendientes para el trabajo en equipo.

## Actividad escolar

**Evidencia 3. Desarrollo colaborativo de un proyecto con control de versiones.**

| Dato | Descripción |
| --- | --- |
| Periodo de desarrollo | 29 de septiembre al 12 de octubre de 2026 |
| Modalidad | Equipo |
| Valor de la evidencia | 50% |
| Evaluación | Heteroevaluación 30% y autoevaluación 20% |
| Entrega | 12 de octubre de 2026 |
| Repositorio | [AlbertoRvlz/Gstionador](https://github.com/AlbertoRvlz/Gstionador) |

El objetivo es desarrollar un sistema pequeño para administrar las tareas de un equipo y demostrar el uso de control de versiones durante el trabajo colaborativo. El historial debe permitir identificar qué hizo cada integrante, cuándo lo hizo, quién revisó sus cambios y cómo se integraron.

## Funcionalidades requeridas

1. Registrar una tarea.
2. Consultar las tareas registradas.
3. Modificar una tarea.
4. Eliminar una tarea.
5. Asignar una tarea a un integrante.
6. Establecer su estado: Pendiente, En proceso o Terminada.
7. Establecer su prioridad: Baja, Media o Alta.
8. Consultar tareas por estado o responsable.

Registro incluye la prioridad y la asignación inicial. Modificación permite cambiar el responsable, el estado y la prioridad. Consulta incluye los filtros. La tabla de reparto indica quién desarrolla cada parte.

## Tecnologías y herramientas

| Herramienta | Uso |
| --- | --- |
| C# y SDK de .NET 10 | Lenguaje y plataforma de ejecución |
| Aplicación de consola | Interfaz del sistema |
| Arquitectura Limpia y DDD | Separación de capas y modelado de las reglas de las tareas |
| Repositorios en memoria | Almacenamiento durante la ejecución |
| Git | Control de versiones local |
| GitHub | Repositorio remoto, pull requests y revisiones |
| Visual Studio compatible o VS Code | Edición y ejecución del código |

No se requiere una base de datos ni paquetes NuGet externos. Los datos se pierden al cerrar la aplicación.

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
- El almacenamiento en memoria pierde los datos al cerrar la aplicación y está pensado para esta consola de un solo usuario. Los métodos del repositorio son infraestructura compartida.

## Eliminación de tareas

Seleccionar la opción 4, ingresar el GUID de la tarea y confirmar con `s`. Enter en el identificador cancela; cualquier respuesta distinta de `s` en la confirmación también cancela. Se aceptan `s` y `S`. Una entrada inválida o una tarea inexistente muestra un mensaje y permite continuar usando el menú.

`EliminarTarea` implementa el caso de uso en Application y recibe `ITareaRepository`. `PantallaEliminarTarea` maneja la entrada y la confirmación en la consola. `Program.cs` conecta ambos usando el mismo repositorio que utilizarán las otras funcionalidades.

La consola inicia sin tareas. Para probar la eliminación desde el menú con una tarea real se necesita integrar la funcionalidad de registro. Las pruebas crean tareas de forma aislada, sin agregar datos de ejemplo a la aplicación.

Ejecutar las pruebas de eliminación (no requieren paquetes externos):

```bash
dotnet run --project tests/GestionTareas.Eliminacion.Tests/GestionTareas.Eliminacion.Tests.csproj
```

El ejecutable verifica eliminación, conservación de otras tareas, identificadores inválidos o inexistentes, cancelación y confirmación. Devuelve código de salida 1 si alguna prueba falla. Se ejecuta con `dotnet run`, no con `dotnet test`.

## Flujo de Git

Usamos Git Flow adaptado al tamaño de esta práctica. Permite desarrollar las cuatro funcionalidades por separado, revisarlas e integrarlas en `develop` antes de llevar una versión funcional a `main`. Así se conserva una rama principal estable y se puede identificar la participación de cada compañero.

```text
main                         Versión estable del proyecto
└── develop                  Integración y pruebas del equipo
    ├── feature/registro-tareas
    ├── feature/consulta-tareas
    ├── feature/modificar-tareas
    └── feature/eliminar
```

Las features se crean desde `develop` y regresan a `develop` mediante pull requests. Después de probar el proyecto integrado, se crea un pull request de `develop` hacia `main`. `main` es la rama principal del repositorio; no se utiliza `master` ni una rama adicional llamada `dev`.

### Acceso y preparación

- El administrador del repositorio debe agregar a Agustín, Joquin, Ángel y Edwin como colaboradores con permiso para subir sus ramas. Cada integrante debe aceptar su invitación.
- Cada integrante trabaja con su propia cuenta de GitHub y configura su nombre y correo de Git antes de hacer commits. El correo debe estar asociado a su cuenta, o ser el correo privado proporcionado por GitHub.
- Comprobar la configuración con `git config user.name` y `git config user.email`. No usar la identidad de otro compañero.
- La confirmación del acceso de los cuatro integrantes debe registrarse en el reporte; este README no acredita que las invitaciones ya hayan sido aceptadas.

### Ramas y commits

- Usar nombres `feature/<funcionalidad>` en minúsculas y con guiones cuando sea necesario.
- No desarrollar funcionalidades directamente en `main` o `develop`.
- Cada compañero crea y sube su propia rama, modifica el código y registra sus avances con commits hechos desde su identidad.
- Hacer commits por cambios concretos y comprobables. No acumular todo el proyecto en un único commit final ni dividir cambios artificialmente solo para aumentar el historial.
- Los mensajes deben explicar el cambio, por ejemplo: `Agregar registro de tareas`, `Validar identificador antes de eliminar` o `Documentar las reglas de trabajo del equipo`.
- Revisar `git diff` y `git status` antes de cada commit. No incluir archivos generados, credenciales ni cambios ajenos a la funcionalidad.
- No reescribir el historial publicado de las ramas compartidas. Conservar los commits individuales durante la integración.
- Eliminar una rama feature cuando su pull request esté integrado y se haya comprobado el resultado. No borrar ramas con cambios pendientes ni eliminar `main` o `develop`.

Cada compañero debe actualizar `develop` antes de crear su rama:

```bash
git switch develop
git pull --ff-only origin develop
git switch -c feature/registro-tareas
```

Cambiar la última rama por la asignada en la tabla. Edwin utiliza `feature/eliminar`.

Al terminar, compilar, revisar cambios y subir la rama:

```bash
dotnet build GestionTareas.slnx
git status
git add <archivos-de-tu-funcionalidad>
git commit -m "Agregar registro de tareas"
git push -u origin feature/registro-tareas
```

Crear un pull request hacia `develop`, describir la funcionalidad y sus comprobaciones. Ajustar la rama y el mensaje al trabajo de cada integrante. La versión integrada y revisada en `develop` se lleva después a `main`.

### Pull requests, revisión e integración

1. El autor sube su rama y abre un pull request hacia `develop`, indicando qué cambió y cómo lo probó.
2. Otro integrante revisa el código, compila y comprueba la funcionalidad. El autor no aprueba su propio trabajo.
3. El revisor registra su aprobación o solicita modificaciones desde GitHub. Si hay observaciones, el autor las atiende con nuevos commits y solicita otra revisión.
4. Integrar únicamente cuando exista al menos una aprobación de otro integrante, se hayan atendido las observaciones, no haya conflictos y las comprobaciones pasen.
5. Usar un merge commit para conservar los commits originales y el registro de integración. Resolver los conflictos en la rama feature y pedir una nueva revisión si cambió el comportamiento.
6. Probar las funcionalidades juntas en `develop`. La integración final hacia `main` también pasa por un pull request revisado por otro integrante.

Estos son acuerdos del equipo. Configurar la protección de ramas en GitHub, si está disponible, para exigir revisión e impedir cambios directos en las ramas compartidas. La configuración de protección no se ha verificado desde este documento.

Cada integrante debe poder mostrar esta secuencia en el historial:

```text
rama propia → modificación de código → commit → push → pull request → revisión → integración
```

## Criterios de entrega

- Registro: crea una tarea con ID único y estado Pendiente; rechaza título vacío, prioridad inválida y responsable inexistente.
- Consulta: lista vacía sin errores, búsqueda por ID y filtros individuales/combinados.
- Modificación: actualiza detalles, responsable, estado y prioridad; rechaza datos inválidos sin dejar cambios parciales; maneja tarea inexistente.
- Eliminación: elimina una tarea existente, confirma el resultado y maneja ID inexistente o mal escrito.
- Integración: registrar → consultar → modificar/asignar → filtrar → eliminar. Compilación sin errores.

El menú tiene eliminación disponible y muestra mensajes de funcionalidad pendiente para registro, consulta y modificación. La aplicación CRUD completa requiere integrar las cuatro ramas.

## Registro de participación

Actualizar esta tabla con enlaces reales a los commits, pull requests y revisiones. El estado corresponde a la evidencia documentada en esta rama; no sustituye la revisión del historial de GitHub.

| Integrante | Actividad asignada | Rama | Commits | Pull request | Revisión realizada | Estado |
| --- | --- | --- | --- | --- | --- | --- |
| Agustín | Registro, prioridad y asignación inicial | `feature/registro-tareas` | Por documentar | Por documentar | Por documentar | Por documentar |
| Joquin | Consulta y filtros por estado o responsable | `feature/consulta-tareas` | Por documentar | Por documentar | Por documentar | Por documentar |
| Ángel | Modificación, responsable, estado y prioridad | `feature/modificar-tareas` | Por documentar | Por documentar | Por documentar | Por documentar |
| Edwin | Eliminación de tareas | `feature/eliminar` | [Implementación de eliminación](https://github.com/AlbertoRvlz/Gstionador/commit/6047286) | Pendiente | Por documentar | Implementada y probada en su rama; pendiente de revisión e integración |

Registrar quién desarrolló, quién revisó y quién integró cada cambio. No marcar aprobaciones ni integraciones que todavía no existan. Cada integrante debe participar directamente; aparecer como colaborador no es suficiente.

## Plan de trabajo hasta la entrega

La actividad establece el periodo del 29 de septiembre al 12 de octubre. Para el cierre del proyecto, la distribución propuesta es:

| Fecha | Actividad prevista |
| --- | --- |
| 8 de octubre | Organización, requisitos, tecnologías y reparto de funcionalidades |
| 9 de octubre | Estructura inicial, reglas de Git, ramas, README y desarrollo individual |
| 10 de octubre | Continuar funcionalidades, commits, push y apertura de pull requests |
| 11 de octubre | Revisión entre compañeros, correcciones, integración y pruebas del conjunto |
| 12 de octubre | Revisión final del historial, reporte y entrega |

Este calendario es una propuesta de organización, no un registro de actividades realizadas. Las fechas reales se comprueban con los commits y las acciones de GitHub.

## Reporte y productos a entregar

El equipo entregará una aplicación funcional y un solo reporte de práctica acompañado del enlace al repositorio. El reporte debe incluir:

- Portada, descripción del proyecto e integrantes del equipo.
- Lenguaje, tecnologías, herramientas y plataforma de control de versiones.
- Flujo seleccionado, justificación y reglas de ramas, commits, pull requests, revisión e integración.
- Distribución de actividades y estructura de ramas.
- Evidencias de desarrollo, commits, pull requests, revisiones, correcciones e integraciones.
- Historial final y tabla resumen de participación individual, con los mismos campos que la tabla de este README.
- Resultado final del proyecto, comprobaciones de funcionamiento, conclusiones y enlace al repositorio.

Las capturas apoyan el reporte, pero el repositorio es la evidencia principal. Guardar enlaces que permitan contrastar cada captura y cada afirmación con el historial real. El README acompaña al reporte; no lo reemplaza.

Antes de entregar, comprobar que los ocho requisitos funcionen en conjunto, que `main` contenga la versión revisada y que cada integrante tenga contribuciones y revisiones identificables.
