namespace PetCore.Infrastructure.Persistence.Configurations;
using PetCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ReceitaConfiguration : IEntityTypeConfiguration<Receita>
{
    public void Configure(EntityTypeBuilder<Receita> builder)
    {
        // Nome da tabela de avaliações.
        builder.ToTable("receita_petcore");

        // Chave primária.
        builder.HasKey(x => x.Id);

        // Atributos
        builder.Property(x => x.IdProntuario)
            .IsRequired();
        
        builder.Property(x => x.IdMedicoResponsavel)
            .IsRequired();

        
        // RELACIONAMENTOS
        //N:N Medicamento
        builder.HasMany(x => x.Medicamentos)
            .WithMany(x => x.Receitas)
            .UsingEntity<Dictionary<string, object>>(
                "rec_medic_petcore",

                right => right.HasOne<Medicamento>()
                    .WithMany()
                    .HasForeignKey("ID_medic_FK")
                    .OnDelete(DeleteBehavior.Cascade),

                left => left.HasOne<Receita>()
                    .WithMany()
                    .HasForeignKey("ID_rec_FK")
                    .OnDelete(DeleteBehavior.NoAction),

                join =>
                {
                    join.ToTable("rec_medic_petcore");
                    join.HasKey("ID_medic_FK", "ID_rec_FK");
                });
        
        //N:1 Prontuario
        builder.HasOne(x => x.Prontuario)
            .WithMany(x => x.Receitas)
            .HasForeignKey(x => x.IdProntuario);
        
        //N:1 Medico
        builder.HasOne(x => x.Medico)
            .WithMany(x => x.Receitas)
            .HasForeignKey(x => x.IdMedicoResponsavel);
    }
}