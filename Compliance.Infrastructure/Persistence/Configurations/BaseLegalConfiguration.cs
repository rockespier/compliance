using Compliance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Compliance.Infrastructure.Persistence.Configurations;

public sealed class BaseLegalConfiguration : IEntityTypeConfiguration<BaseLegal>
{
    public void Configure(EntityTypeBuilder<BaseLegal> builder)
    {
        builder.ToTable("BasesLegales");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Codigo)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Descripcion)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(x => x.Organismo)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Tipo)
            .HasMaxLength(100)
            .IsRequired();
    }
}