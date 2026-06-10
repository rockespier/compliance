# Tareas pendientes

## Crear nueva base legal

- [ ] Dominio: crear o ajustar la entidad `BaseLegal` con sus validaciones y reglas de negocio.
- [ ] Aplicación: crear el caso de uso para registrar una nueva base legal y sus DTOs de entrada/salida.
- [ ] Infraestructura: agregar el repositorio o método de persistencia para guardar la base legal.
- [ ] Infraestructura: revisar o agregar la configuración de EF Core y crear migración si aplica.
- [ ] API: exponer un endpoint para crear una nueva base legal.
- [ ] Tests: agregar pruebas unitarias y de integración para el flujo de creación de base legal.

## Crear nuevo proyecto

- [ ] Dominio: crear o ajustar la entidad `Proyecto` con sus validaciones y reglas de negocio.
- [ ] Aplicación: crear el caso de uso para registrar un nuevo proyecto y sus DTOs de entrada/salida.
- [ ] Infraestructura: agregar el repositorio o método de persistencia para guardar el proyecto.
- [ ] Infraestructura: revisar o agregar la configuración de EF Core y crear migración si aplica.
- [ ] API: exponer un endpoint para crear un nuevo proyecto.
- [ ] Tests: agregar pruebas unitarias y de integración para el flujo de creación de proyecto.

## Relacionar base legal con el proyecto

- [ ] Dominio: definir la regla para relacionar una base legal con un proyecto según ubicación, tipo y otros criterios aplicables.
- [ ] Dominio: validar si la relación crea directamente un `PlanDeCumplimiento` o una asociación previa.
- [ ] Aplicación: crear el caso de uso para relacionar base legal y proyecto.
- [ ] Aplicación: definir DTOs y validaciones para la solicitud de relación.
- [ ] Infraestructura: agregar consultas y persistencia para obtener entidades y guardar la relación.
- [ ] Infraestructura: revisar configuraciones de EF Core y crear migración si aplica.
- [ ] API: exponer un endpoint para ejecutar la relación entre base legal y proyecto.
- [ ] Tests: agregar pruebas unitarias y de integración para reglas de relación y escenarios inválidos.
