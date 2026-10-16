using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCore.Domain.Entities;

namespace PetCore.Infrastructure.Persistence.Configurations;

public class ProtocoloConfiguration : IEntityTypeConfiguration<Protocolo>
{
    public void Configure(EntityTypeBuilder<Protocolo> builder)
    {
        builder.ToTable("protocolo_petcore");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasMaxLength(20);
        builder.Property(x => x.Titulo).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Texto).IsRequired();
    }
}
