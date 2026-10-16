namespace PetCore.Infrastructure.Persistence.Configurations;
using PetCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class EnderecoConfiguration : IEntityTypeConfiguration<Endereco>
{
    public void Configure(EntityTypeBuilder<Endereco> builder)
    {
        // Nome da tabela de avaliações.
        builder.ToTable("endereco_petcore");

        // Chave primária.
        builder.HasKey(x => x.Id);

        // Atributos
        builder.Property(x => x.Cep)
            .IsRequired();
        
        builder.Property(x => x.Complemento)
            .IsRequired();

        
        // RELACIONAMENTOS
        //1:1 Clinica (ClinicaConfiguration)

    }
}