using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unimake.DFe.Database.Models;

namespace Unimake.DFe.Database.Data.Mapping;

/// <summary>
/// Configuração de mapeamento para ItemNFeEntity
/// </summary>
public class ItemNFeMapping : IEntityTypeConfiguration<ItemNFeEntity>
{
    public void Configure(EntityTypeBuilder<ItemNFeEntity> builder)
    {
        // Configuração da tabela
        builder.ToTable("item_nfe");

        // Chave primária
        builder.HasKey(e => e.Id);

        // Mapeamento das colunas para snake_lowercase
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.NumeroItem).HasColumnName("numero_item");
        builder.Property(e => e.CodigoProduto).HasColumnName("codigo_produto").IsRequired().HasMaxLength(60);
        builder.Property(e => e.GTIN).HasColumnName("gtin").HasMaxLength(14);
        builder.Property(e => e.Descricao).HasColumnName("descricao").IsRequired().HasMaxLength(120);
        builder.Property(e => e.NCM).HasColumnName("ncm").HasMaxLength(8);
        builder.Property(e => e.NVE).HasColumnName("nve").HasMaxLength(6);
        builder.Property(e => e.CEST).HasColumnName("cest").HasMaxLength(7);
        builder.Property(e => e.IndEscala).HasColumnName("ind_escala").HasMaxLength(1);
        builder.Property(e => e.CNPJFab).HasColumnName("cnpj_fab").HasMaxLength(14);
        builder.Property(e => e.CBenef).HasColumnName("c_benef").HasMaxLength(10);
        builder.Property(e => e.EXTIPI).HasColumnName("ex_tipi").HasMaxLength(3);
        builder.Property(e => e.CFOP).HasColumnName("cfop").IsRequired().HasMaxLength(4);
        builder.Property(e => e.UnidadeComercial).HasColumnName("unidade_comercial").IsRequired().HasMaxLength(6);
        builder.Property(e => e.QuantidadeComercial).HasColumnName("quantidade_comercial").HasColumnType("decimal(15,4)");
        builder.Property(e => e.ValorUnitarioComercial).HasColumnName("valor_unitario_comercial").HasColumnType("decimal(21,10)");
        builder.Property(e => e.ValorTotal).HasColumnName("valor_total").HasColumnType("decimal(15,2)");
        builder.Property(e => e.GTINTributavel).HasColumnName("gtin_tributavel").HasMaxLength(14);
        builder.Property(e => e.UnidadeTributavel).HasColumnName("unidade_tributavel").HasMaxLength(6);
        builder.Property(e => e.QuantidadeTributavel).HasColumnName("quantidade_tributavel").HasColumnType("decimal(15,4)");
        builder.Property(e => e.ValorUnitarioTributavel).HasColumnName("valor_unitario_tributavel").HasColumnType("decimal(21,10)");
        builder.Property(e => e.ValorFrete).HasColumnName("valor_frete").HasColumnType("decimal(15,2)");
        builder.Property(e => e.ValorSeguro).HasColumnName("valor_seguro").HasColumnType("decimal(15,2)");
        builder.Property(e => e.ValorDesconto).HasColumnName("valor_desconto").HasColumnType("decimal(15,2)");
        builder.Property(e => e.OutrasDespesas).HasColumnName("outras_despesas").HasColumnType("decimal(15,2)");
        builder.Property(e => e.IndicadorTotal).HasColumnName("indicador_total");
        builder.Property(e => e.PedidoCompra).HasColumnName("pedido_compra").HasMaxLength(15);
        builder.Property(e => e.ItemPedidoCompra).HasColumnName("item_pedido_compra");
        builder.Property(e => e.NumeroFCI).HasColumnName("numero_fci").HasMaxLength(36);
        builder.Property(e => e.DataCriacao).HasColumnName("data_criacao");
        builder.Property(e => e.DataAtualizacao).HasColumnName("data_atualizacao");
        builder.Property(e => e.NFeId).HasColumnName("nfe_id");

        // Índices
        builder.HasIndex(e => new { e.NFeId, e.NumeroItem }).IsUnique();

        // Relacionamentos
        builder.HasOne(i => i.NFe)
               .WithMany(n => n.Itens)
               .HasForeignKey(i => i.NFeId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}