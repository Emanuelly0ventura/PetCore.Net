namespace PetCore.Infrastructure.Persistence.Configurations;

using PetCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ProntuarioConfiguration : IEntityTypeConfiguration<Prontuario>
{
    public void Configure(EntityTypeBuilder<Prontuario> builder)
    {
        // Nome da tabela de avaliações.
        builder.ToTable("prontuario_petcore");

        // Chave primária.
        builder.HasKey(x => x.Id);

        // Atributos
        builder.Property(x => x.DataEmissao)
            .IsRequired();
        
        builder.Property(x => x.Descricao)
            .IsRequired();
        
        builder.Property(x => x.IdMedico)
            .IsRequired();
        
        builder.Property(x => x.IdHistorico)
            .IsRequired();
        
        // RELACIONAMENTOS
        //N:N Receita (ReceitaConfiguration)
        //N:N Exame (ExameConfiguration)
        
        //N:1 Historico
        builder.HasOne(x => x.Historico)
            .WithMany(x => x.Prontuarios)
            .HasForeignKey(x => x.IdHistorico);
        
        //N:1 Medico
        builder.HasOne(x => x.Medico)
            .WithMany(x => x.Prontuarios)
            .HasForeignKey(x => x.IdMedico);
    }
}