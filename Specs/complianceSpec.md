# Especificación: Core de Cumplimiento Legal 

## 1. Lenguaje Ubicuo y Diccionario de Datos Estricto
Para garantizar consistencia, las entidades del dominio deben contener **estrictamente** las siguientes propiedades y tipos. No agregar campos adicionales.

* **`BaseLegal`**:
    * `Id` (Guid)
    * `Codigo` (string)
    * `Descripcion` (string)
    * `Organismo` (string) - Ej: OEFA, MEM, MINEM, Tributario, Laboral.
    * `Tipo` (string) - Ej: Ambiental, Laboral, Tributario.

* **`Proyecto`**:
    * `Id` (Guid)
    * `Nombre` (string)
    * `Ubicacion` (string)
    * `ZonaSierra` (bool)
    * `FaseProyecto` (string)

* **`PlanDeCumplimiento`**:
    * `Id` (Guid)
    * `BaseLegalId` (Guid)
    * `ProyectoId` (Guid)
    * `Responsable` (string)
    * `FechaLimite` (DateTimeOffset)

* **`RegistroDeCumplimiento`**:
    * `Id` (Guid)
    * `PlanCumplimientoId` (Guid)
    * `Estado` (Enum: Pendiente, Cumplido, Vencido)
    * `FechaRealCumplimiento` (DateTimeOffset nullable)
    * `Evidencia` (Value Object o Entidad anidada que contenga: `NombreDocumento` string, `Url` string, `FechaAdjunto` DateTimeOffset).

## 2. Reglas de Negocio Estrictas (Domain Logic)
* **Encapsulamiento de Estado:** Un `RegistroCumplimiento` no puede cambiar su estado a "Cumplido" modificando una propiedad pública. Debe existir un método llamado `MarcarComoCumplido(Evidencia evidencia)` que valide la regla de negocio.
* **Validación de Evidencia:** Es obligatorio adjuntar certificados (ej. OEFA, permisos MEM, licencia social) para que un registro se considere cumplido. Si el método `MarcarComoCumplido` no recibe una Evidencia válida, debe lanzar una excepción de dominio.
* **Alertas:** El sistema (o la entidad `PlanCumplimiento`) debe ser capaz de calcular e identificar si se encuentra en estado de alerta (faltan 30, 15 o 7 días para su vencimiento).

## 3. Directrices Técnicas (Clean Architecture)
* **Stack:** Usar C# 13 y .NET 9.
* **Encapsulamiento:** Aplicar encapsulamiento estricto. Las propiedades deben tener `get` público y `private set` (o `init`). Fomentar el uso de constructores primarios (Primary Constructors) de C#.
* **Pureza del Dominio:** El proyecto `Compliance.Domain` NO DEBE tener dependencias externas (solo BCL - Base Class Library). 
* **Entity Framework Core:** NO usar Data Annotations (como `[Table]` o `[Column]`) en las entidades del Dominio. Toda la configuración de base de datos se hará en el proyecto de Infraestructura utilizando Fluent API (`IEntityTypeConfiguration`).