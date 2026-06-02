using Compliance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Compliance.Infrastructure.Persistence.Configurations;

public sealed class ProyectoConfiguration : IEntityTypeConfiguration<Proyecto>
{
    public void Configure(EntityTypeBuilder<Proyecto> builder)
    {
        builder.ToTable("Proyectos");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Nombre)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Ubicacion)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Fase)
            .HasMaxLength(100)
            .IsRequired();
    }
}