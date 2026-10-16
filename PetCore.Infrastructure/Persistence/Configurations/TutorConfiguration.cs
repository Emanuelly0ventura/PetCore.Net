using PetCore.Domain.Entities;

namespace PetCore.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Mapeamento Fluent API da entidade <see cref="Tutor"/>.
/// </summary>
public class TutorConfiguration : IEntityTypeConfiguration<Tutor>
{
    /// <summary>
    /// Configura colunas, índice único e FKs de avaliação.
    /// </summary>
    public void Configure(EntityTypeBuilder<Tutor> builder)
    {
        // Nome da tabela de avaliações.
        builder.ToTable("tutor_petcore");

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
        
        // Relacionamento com pet (N:N).
        builder.HasMany(x => x.Pets)
            .WithMany(x => x.Tutores)
            .UsingEntity<Dictionary<string, object>>(
                "tut_pet_petcore",

                right => right.HasOne<Pet>()
                    .WithMany()
                    .HasForeignKey("ID_pet_FK")
                    .OnDelete(DeleteBehavior.Cascade),

                left => left.HasOne<Tutor>()
                    .WithMany()
                    .HasForeignKey("ID_tut_FK")
                    .OnDelete(DeleteBehavior.NoAction),

                join =>
                {
                    join.ToTable("tut_pet_petcore");
                    join.HasKey("ID_tut_FK", "ID_pet_FK");
                });
    }
}