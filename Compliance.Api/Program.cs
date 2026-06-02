// 1. Necesario para que funcione UseSqlServer()
using Microsoft.EntityFrameworkCore;

// 2. Necesario para que la API conozca AppDbContext
using Compliance.Infrastructure;
using Compliance.Infrastructure.Repositories;

// 3. Necesario para que la API conozca los Casos de Uso y DTOs
using Compliance.Application;
using Compliance.Application.Abstractions;
using Compliance.Application.UseCases;
using System;
using Compliance.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// ============================================================
// PASO 4.1: CONFIGURACIÓN DE INFRAESTRUCTURA E INYECCIÓN (DI)
// ============================================================

// 1. Registrar el DbContext usando el Connection String del appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Registrar los repositorios y casos de uso (Scoped porque viven por cada petición HTTP)
// Nota: 'PlanCumplimientoRepository' es la clase concreta que generó Copilot en Infrastructure
builder.Services.AddScoped<IPlanCumplimientoRepository, PlanCumplimientoRepository>();
builder.Services.AddScoped<MarcarCumplimientoUseCase>();

var app = builder.Build();

// ============================================================
// PASO 5: LA CAPA DE PRESENTACIÓN (MINIMAL APIs)
// ============================================================

// Endpoint para registrar el cumplimiento de un plan
app.MapPost("/api/planes/{planId:guid}/cumplimiento", async (
    Guid planId,
    MarcarCumplimientoDto request, // Usamos el DTO que generó la IA
    MarcarCumplimientoUseCase useCase,
    CancellationToken cancellationToken) => // La IA agregó esto, ¡aprovechémoslo!
{
    try
    {
        // Llamamos al método exacto que generó Copilot
        await useCase.ExecuteAsync(planId, request, cancellationToken);

        return Results.Ok(new { Mensaje = "Cumplimiento registrado exitosamente." });
    }
    catch (KeyNotFoundException ex)
    {
        // La IA lanza esto si no encuentra el plan
        return Results.NotFound(new { Error = ex.Message });
    }
    catch (ArgumentException ex)
    {
        // La IA lanza esto si faltan datos
        return Results.BadRequest(new { Error = ex.Message });
    }
    catch (Exception ex)
    {
        return Results.Problem(detail: ex.Message, statusCode: 500);
    }
});

app.Run();