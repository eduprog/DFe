using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unimake.DFe.Database.Models;

namespace Unimake.DFe.Database.Data.Mapping;

/// <summary>
/// Configuração de mapeamento para EnviNFeEntity
/// </summary>
public class EnviNFeMapping : IEntityTypeConfiguration<EnviNFeEntity>
{
    public void Configure(EntityTypeBuilder<EnviNFeEntity> builder)
    {
        // Configuração da tabela
        builder.ToTable("envi_nfe");

        // Chave primária
        builder.HasKey(e => e.Id);

        // Mapeamento das colunas para snake_lowercase
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Versao).HasColumnName("versao").HasMaxLength(10);
        builder.Property(e => e.IdLote).HasColumnName("id_lote").IsRequired().HasMaxLength(15);
        builder.Property(e => e.IndSinc).HasColumnName("ind_sinc");
        builder.Property(e => e.DataCriacao).HasColumnName("data_criacao");
        builder.Property(e => e.DataAtualizacao).HasColumnName("data_atualizacao");
        builder.Property(e => e.Status).HasColumnName("status").HasMaxLength(50);
        builder.Property(e => e.Observacoes).HasColumnName("observacoes").HasMaxLength(1000);

        // Índices
        builder.HasIndex(e => e.IdLote).IsUnique();
        builder.HasIndex(e => e.DataCriacao);
    }
}
