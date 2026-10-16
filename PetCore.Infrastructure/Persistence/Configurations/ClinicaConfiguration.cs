namespace PetCore.Infrastructure.Persistence.Configurations;
using PetCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ClinicaConfiguration: IEntityTypeConfiguration<Clinica>
{
    public void Configure(EntityTypeBuilder<Clinica> builder)
    {
        // Nome da tabela de avaliações.
        builder.ToTable("clinica_petcore");

        // Chave primária.
        builder.HasKey(x => x.Id);

        // Atributos
        builder.Property(x => x.Nome)
            .IsRequired();
        
        builder.Property(x => x.Cnpj)
            .IsRequired();
        
        builder.Property(x => x.IdEndereco)
            .IsRequired();
        
        // RELACIONAMENTOS
        //N:N Relatorio (RelatorioConfiguration)
        
        //1:1 Endereco
        builder.HasOne(x => x.Endereco)
            .WithOne(x => x.Clinica)
            .HasForeignKey<Clinica>(x => x.IdEndereco);
        
    }
}