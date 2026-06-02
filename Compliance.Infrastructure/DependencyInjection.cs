using Compliance.Application.Abstractions;
using Compliance.Infrastructure.Persistence;
using Compliance.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Compliance.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        Action<DbContextOptionsBuilder> configureDb)
    {
        services.AddDbContext<AppDbContext>(configureDb);
        services.AddScoped<IPlanCumplimientoRepository, PlanCumplimientoRepository>();
        return services;
    }
}