namespace PetCore.Infrastructure.Persistence.Configurations;

using PetCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        // Nome da tabela de avaliações.
        builder.ToTable("pet_petcore");

        // Chave primária.
        builder.HasKey(x => x.Id);

        // Atributos
        builder.Property(x => x.Nome)
            .IsRequired();
        
        builder.Property(x => x.Especie)
            .IsRequired();
        
        builder.Property(x => x.Raca)
            .IsRequired();
        
        builder.Property(x => x.DataNasc)
            .IsRequired();
        
        builder.Property(x => x.Pelagem)
            .IsRequired();
        
        builder.Property(x => x.Porte)
            .IsRequired();
        
        builder.Property(x => x.Sexo)
            .IsRequired();
        
        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.UrlImg);
        
        // RELACIONAMENTOS
        //N:N Tutor (TutorConfiguration)
    }
}
