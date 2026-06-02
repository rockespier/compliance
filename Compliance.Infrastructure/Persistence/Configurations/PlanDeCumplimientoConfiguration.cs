using Compliance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Compliance.Infrastructure.Persistence.Configurations;

public sealed class PlanDeCumplimientoConfiguration : IEntityTypeConfiguration<PlanDeCumplimiento>
{
    public void Configure(EntityTypeBuilder<PlanDeCumplimiento> builder)
    {
        builder.ToTable("PlanesCumplimiento");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Responsable)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.FechaLimite)
            .IsRequired();

        builder.Property<Guid>("BaseLegalId").IsRequired();
        builder.Property<Guid>("ProyectoId").IsRequired();

        builder.HasOne(x => x.BaseLegal)
            .WithMany()
            .HasForeignKey("BaseLegalId")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Proyecto)
            .WithMany()
            .HasForeignKey("ProyectoId")
            .OnDelete(DeleteBehavior.Restrict);
    }
}