using Compliance.Domain.Entities;
using Compliance.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Compliance.Infrastructure.Persistence.Configurations;

public sealed class RegistroDeCumplimientoConfiguration : IEntityTypeConfiguration<RegistroDeCumplimiento>
{
    public void Configure(EntityTypeBuilder<RegistroDeCumplimiento> builder)
    {
        builder.ToTable("RegistrosCumplimiento");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Estado)
            .HasConversion(
                v => v.ToString(),
                v => Enum.Parse<EstadoCumplimiento>(v))
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.FechaCumplimiento);

        // FK shadow verso Plan
        builder.Property<Guid>("PlanId")
            .IsRequired();

        builder.HasIndex("PlanId")
            .IsUnique();

        builder.HasOne(x => x.Plan)
            .WithMany()
            .HasForeignKey("PlanId")
            .OnDelete(DeleteBehavior.Cascade);

        // Mapping della navigation collection usando il backing field
        builder.Navigation(x => x.Evidencias)
            .HasField("_evidencias")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany<Evidencia>(x => x.Evidencias)
            .WithOne()
            .HasForeignKey("RegistroDeCumplimientoId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}