using Compliance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Compliance.Infrastructure.Persistence.Configurations;

public sealed class EvidenciaConfiguration : IEntityTypeConfiguration<Evidencia>
{
    public void Configure(EntityTypeBuilder<Evidencia> builder)
    {
        builder.ToTable("Evidencias");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Identificador)
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(x => x.Descripcion)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(x => x.FechaAdjunto)
            .IsRequired();
    }
}