using Compliance.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Compliance.Infrastructure.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<BaseLegal> BasesLegales => Set<BaseLegal>();
    public DbSet<Proyecto> Proyectos => Set<Proyecto>();
    public DbSet<PlanDeCumplimiento> PlanesCumplimiento => Set<PlanDeCumplimiento>();
    public DbSet<RegistroDeCumplimiento> RegistrosCumplimiento => Set<RegistroDeCumplimiento>();
    public DbSet<Evidencia> Evidencias => Set<Evidencia>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}