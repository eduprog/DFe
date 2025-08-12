using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unimake.DFe.Database.Models;

namespace Unimake.DFe.Database.Data.Mapping;

/// <summary>
/// Configuração de mapeamento para NFeEntity
/// </summary>
public class NFeMapping : IEntityTypeConfiguration<NFeEntity>
{
    public void Configure(EntityTypeBuilder<NFeEntity> builder)
    {
        // Configuração da tabela
        builder.ToTable("nfe");

        // Chave primária
        builder.HasKey(e => e.Id);

        // Mapeamento das colunas para snake_lowercase
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.ChaveAcesso).HasColumnName("chave_acesso").IsRequired().HasMaxLength(44);
        builder.Property(e => e.NumeroNFe).HasColumnName("numero_nfe");
        builder.Property(e => e.Serie).HasColumnName("serie");
        builder.Property(e => e.Modelo).HasColumnName("modelo");
        builder.Property(e => e.DataEmissao).HasColumnName("data_emissao");
        builder.Property(e => e.DataSaidaEntrada).HasColumnName("data_saida_entrada");
        builder.Property(e => e.TipoOperacao).HasColumnName("tipo_operacao");
        builder.Property(e => e.CodigoUF).HasColumnName("codigo_uf");
        builder.Property(e => e.TipoEmissao).HasColumnName("tipo_emissao");
        builder.Property(e => e.TipoAmbiente).HasColumnName("tipo_ambiente");
        builder.Property(e => e.FinalidadeNFe).HasColumnName("finalidade_nfe");
        builder.Property(e => e.IndicadorConsumidorFinal).HasColumnName("indicador_consumidor_final");
        builder.Property(e => e.IndicadorPresenca).HasColumnName("indicador_presenca");
        builder.Property(e => e.NaturezaOperacao).HasColumnName("natureza_operacao").HasMaxLength(60);
        builder.Property(e => e.XmlCompleto).HasColumnName("xml_completo");
        builder.Property(e => e.Status).HasColumnName("status").HasMaxLength(50);
        builder.Property(e => e.DataCriacao).HasColumnName("data_criacao");
        builder.Property(e => e.DataAtualizacao).HasColumnName("data_atualizacao");
        builder.Property(e => e.EnviNFeId).HasColumnName("envi_nfe_id");
        builder.Property(e => e.EmitenteId).HasColumnName("emitente_id");
        builder.Property(e => e.DestinatarioId).HasColumnName("destinatario_id");

        // Índices
        builder.HasIndex(e => e.ChaveAcesso).IsUnique();
        builder.HasIndex(e => e.NumeroNFe);
        builder.HasIndex(e => e.DataEmissao);
        builder.HasIndex(e => e.TipoAmbiente);

        // Relacionamentos
        builder.HasOne(e => e.EnviNFe)
               .WithMany(en => en.NFes)
               .HasForeignKey(e => e.EnviNFeId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Emitente)
               .WithMany(em => em.NFesEmitidas)
               .HasForeignKey(e => e.EmitenteId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.Destinatario)
               .WithMany(d => d.NFesRecebidas)
               .HasForeignKey(e => e.DestinatarioId)
               .OnDelete(DeleteBehavior.SetNull);
    }
}