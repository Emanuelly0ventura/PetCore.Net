
namespace PetCore.Infrastructure.Persistence.Configurations;

using PetCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class RelatorioConfiguration : IEntityTypeConfiguration<Relatorio>
{
    public void Configure(EntityTypeBuilder<Relatorio> builder)
    {
        // Nome da tabela de avaliações.
        builder.ToTable("relatorio_petcore");

        // Chave primária.
        builder.HasKey(x => x.Id);

        // Atributos
        builder.Property(x => x.Observacao)
            .IsRequired();
        
        builder.Property(x => x.IdHistorico)
            .IsRequired();
        
        builder.Property(x => x.IdMedicoResponsavel)
            .IsRequired();
        
        // RELACIONAMENTOS
        //N:N Clinica
        builder.HasMany(x => x.Clinicas)
            .WithMany(x => x.Relatorios)
            .UsingEntity<Dictionary<string, object>>(
                "rel_cli_petcore",

                right => right.HasOne<Clinica>()
                    .WithMany()
                    .HasForeignKey("ID_cli_FK")
                    .OnDelete(DeleteBehavior.Cascade),

                left => left.HasOne<Relatorio>()
                    .WithMany()
                    .HasForeignKey("ID_rel_FK")
                    .OnDelete(DeleteBehavior.NoAction),

                join =>
                {
                    join.ToTable("rel_cli_petcore");
                    join.HasKey("ID_cli_FK", "ID_rel_FK");
                });
        
        //N:1 Historico
        builder.HasOne(x => x.Historico)
            .WithMany(x => x.Relatorios)
            .HasForeignKey(x => x.IdHistorico);
        
        //N:1 Medico
        builder.HasOne(x => x.Medico)
            .WithMany(x => x.Relatorios)
            .HasForeignKey(x => x.IdMedicoResponsavel);
    }
}