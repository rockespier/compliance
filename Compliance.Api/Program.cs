// 1. Necesario para que funcione UseSqlServer()
// 3. Necesario para que la API conozca los Casos de Uso y DTOs
using Compliance.Application;
using Compliance.Application.Abstractions;
using Compliance.Application.UseCases;
using Compliance.Application.UseCases.PlanCumplimiento;
// 2. Necesario para que la API conozca AppDbContext
using Compliance.Infrastructure;
using Compliance.Infrastructure.Persistence;
using Compliance.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using System;

var builder = WebApplication.CreateBuilder(args);

// ============================================================
// PASO 4.1: CONFIGURACI�N DE INFRAESTRUCTURA E INYECCI�N (DI)
// ============================================================

// 1. Registrar el DbContext usando el Connection String del appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Registrar los repositorios y casos de uso (Scoped porque viven por cada petici�n HTTP)
// Nota: 'PlanCumplimientoRepository' es la clase concreta que gener� Copilot en Infrastructure
builder.Services.AddScoped<IPlanCumplimientoRepository, PlanCumplimientoRepository>();
builder.Services.AddScoped<MarcarCumplimientoUseCase>();

builder.Services.AddScoped<CrearPlanCumplimientoUseCase>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Compliance API", Version = "v1" });
});


var app = builder.Build();
var swaggerEnabled = app.Environment.IsDevelopment();

if (swaggerEnabled)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// En desarrollo redirigimos a Swagger; en otros entornos devolvemos un estado simple.
app.MapGet("/", () =>
    swaggerEnabled
        ? Results.Redirect("/swagger")
        : Results.Ok(new { Mensaje = "Compliance API en ejecucion." }))
    .ExcludeFromDescription();


// ============================================================
// PASO 5: LA CAPA DE PRESENTACI�N (MINIMAL APIs)
// ============================================================

// Creamos un endpoint POST que recibe el ID del plan por la URL y los datos por el Body (JSON)
app.MapPost("/api/planes/{planId:guid}/cumplimiento", async (
    Guid planId,
    MarcarCumplimientoDto request, // El DTO inmutable que trae los datos de la red
    MarcarCumplimientoUseCase useCase, // Inyectamos nuestro caso de uso
    CancellationToken cancellationToken) => // Token para cancelar peticiones largas
{
    try
    {
        // 1. Delegamos toda la responsabilidad al Caso de Uso
        await useCase.ExecuteAsync(planId, request, cancellationToken);

        // 2. Si todo sale bien, devolvemos un HTTP 200 OK
        return Results.Ok(new { Mensaje = "Cumplimiento registrado exitosamente." });
    }
    catch (KeyNotFoundException ex)
    {
        // Si el plan no existe en la BD, devolvemos un HTTP 404 Not Found
        return Results.NotFound(new { Error = ex.Message });
    }
    catch (ArgumentException ex)
    {
        // Si hay un error de validaci�n (ej. falta evidencia), devolvemos HTTP 400 Bad Request
        return Results.BadRequest(new { Error = ex.Message });
    }
    catch (Exception ex)
    {
        // Si ocurre cualquier otro error inesperado, devolvemos HTTP 500 Internal Server Error
        return Results.Problem(detail: ex.Message, statusCode: 500);
    }
});

// Endpoint para CREAR un nuevo plan de cumplimiento
app.MapPost("/api/planes", async (
    CrearPlanCumplimientoDto request,
    CrearPlanCumplimientoUseCase useCase,
    CancellationToken cancellationToken) =>
{
    try
    {
        var nuevoPlanId = await useCase.ExecuteAsync(request, cancellationToken);

        // Retornamos un 201 Created junto con el ID del nuevo recurso
        return Results.Created($"/api/planes/{nuevoPlanId}", new { Id = nuevoPlanId, Mensaje = "Plan creado con �xito." });
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { Error = ex.Message });
    }
    catch (Exception ex)
    {
        // Si ocurre cualquier otro error inesperado, devolvemos HTTP 500 Internal Server Error
        return Results.Problem(detail: ex.Message, statusCode: 500);
    }
});

app.Run();
