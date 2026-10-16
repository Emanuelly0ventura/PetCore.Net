namespace PetCore.Infrastructure.Persistence.Configurations;
using PetCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ExameConfiguration : IEntityTypeConfiguration<Exame>
{
    public void Configure(EntityTypeBuilder<Exame> builder)
    {
        // Nome da tabela de avaliações.
        builder.ToTable("exame_petcore");

        // Chave primária.
        builder.HasKey(x => x.Id);

        // Atributos
        builder.Property(x => x.Nome)
            .IsRequired();
        
        builder.Property(x => x.IdMedico)
            .IsRequired();
        
        builder.Property(x => x.IdProntuario)
            .IsRequired();
        
        // RELACIONAMENTOS
        //N:1 Prontuario
        builder.HasOne(x => x.Prontuario)
            .WithMany(x => x.Exames)
            .HasForeignKey(x => x.IdProntuario);
        
        //N:1 Medico
        builder.HasOne(x => x.Medico)
            .WithMany(x => x.Exames)
            .HasForeignKey(x => x.IdMedico);
    }
}