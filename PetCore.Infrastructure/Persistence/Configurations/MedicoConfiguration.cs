namespace PetCore.Infrastructure.Persistence.Configurations;

using PetCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class MedicoConfiguration : IEntityTypeConfiguration<Medico>
{
    public void Configure(EntityTypeBuilder<Medico> builder)
    {
        // Nome da tabela de avaliações.
        builder.ToTable("medico_petcore");

        // Chave primária.
        builder.HasKey(x => x.Id);

        // Atributos
        builder.Property(x => x.Nome)
            .IsRequired();
        
        builder.Property(x => x.DataNascimento)
            .IsRequired();
        
        builder.Property(x => x.Telefone)
            .IsRequired();
        
        builder.Property(x => x.Email)
            .IsRequired();
        
        builder.Property(x => x.Sexo)
            .HasConversion<string>()
            .HasMaxLength(1);

        builder.Property(x => x.Senha)
            .IsRequired();

        builder.Property(x => x.UrlImg);
        
        builder.Property(x => x.Especialidade)
            .IsRequired();

        builder.Property(x => x.Clinica).HasMaxLength(200);
        builder.Property(x => x.NomeClinica).HasMaxLength(200);
        builder.Property(x => x.CnpjClinica).HasMaxLength(20);
        builder.Property(x => x.CepClinica).HasMaxLength(20);
        builder.Property(x => x.ComplementoClinica).HasMaxLength(200);
        
        // RELACIONAMENTOS
        //1:N Relatorio (RelatorioConfiguration)
        
        //1:N Receita (ReceitaConfiguration)

        //1:N Exame (ExameConfiguration)
        
        //1:N Prontuario (ProntuarioConfiguration)
        
    }
}
