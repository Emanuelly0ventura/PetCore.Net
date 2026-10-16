namespace PetCore.Infrastructure.Persistence.Configurations;
using PetCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class MedicamentoConfiguration : IEntityTypeConfiguration<Medicamento>
{
    public void Configure(EntityTypeBuilder<Medicamento> builder)
    {
        // Nome da tabela de avaliações.
        builder.ToTable("medicamento_petcore");

        // Chave primária.
        builder.HasKey(x => x.Id);

        // Atributos
        builder.Property(x => x.Nome)
            .IsRequired();
        
        builder.Property(x => x.Dosagem)
            .IsRequired();
        
        builder.Property(x => x.Instrucao)
            .IsRequired();
        
        // RELACIONAMENTOS
        //N:N Receita (ReceitaPetcore)
    }
}