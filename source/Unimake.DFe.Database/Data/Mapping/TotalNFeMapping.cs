using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unimake.DFe.Database.Models;

namespace Unimake.DFe.Database.Data.Mapping;

/// <summary>
/// Configuração de mapeamento para TotalNFeEntity
/// </summary>
public class TotalNFeMapping : IEntityTypeConfiguration<TotalNFeEntity>
{
    public void Configure(EntityTypeBuilder<TotalNFeEntity> builder)
    {
        // Configuração da tabela
        builder.ToTable("total_nfe");

        // Chave primária
        builder.HasKey(e => e.Id);

        // Mapeamento das colunas para snake_lowercase
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.BaseCalculoICMS).HasColumnName("base_calculo_icms").HasColumnType("decimal(15,2)");
        builder.Property(e => e.ValorICMS).HasColumnName("valor_icms").HasColumnType("decimal(15,2)");
        builder.Property(e => e.ValorICMSDesonerado).HasColumnName("valor_icms_desonerado").HasColumnType("decimal(15,2)");
        builder.Property(e => e.BaseCalculoFCP).HasColumnName("base_calculo_fcp").HasColumnType("decimal(15,2)");
        builder.Property(e => e.ValorFCP).HasColumnName("valor_fcp").HasColumnType("decimal(15,2)");
        builder.Property(e => e.BaseCalculoICMSST).HasColumnName("base_calculo_icms_st").HasColumnType("decimal(15,2)");
        builder.Property(e => e.ValorICMSST).HasColumnName("valor_icms_st").HasColumnType("decimal(15,2)");
        builder.Property(e => e.BaseCalculoFCPST).HasColumnName("base_calculo_fcp_st").HasColumnType("decimal(15,2)");
        builder.Property(e => e.ValorFCPST).HasColumnName("valor_fcp_st").HasColumnType("decimal(15,2)");
        builder.Property(e => e.BaseCalculoFCPSTRet).HasColumnName("base_calculo_fcp_st_ret").HasColumnType("decimal(15,2)");
        builder.Property(e => e.ValorFCPSTRet).HasColumnName("valor_fcp_st_ret").HasColumnType("decimal(15,2)");
        builder.Property(e => e.ValorTotalProdutos).HasColumnName("valor_total_produtos").HasColumnType("decimal(15,2)");
        builder.Property(e => e.ValorFrete).HasColumnName("valor_frete").HasColumnType("decimal(15,2)");
        builder.Property(e => e.ValorSeguro).HasColumnName("valor_seguro").HasColumnType("decimal(15,2)");
        builder.Property(e => e.ValorDesconto).HasColumnName("valor_desconto").HasColumnType("decimal(15,2)");
        builder.Property(e => e.ValorII).HasColumnName("valor_ii").HasColumnType("decimal(15,2)");
        builder.Property(e => e.ValorIPI).HasColumnName("valor_ipi").HasColumnType("decimal(15,2)");
        builder.Property(e => e.ValorIPIDevolvido).HasColumnName("valor_ipi_devolvido").HasColumnType("decimal(15,2)");
        builder.Property(e => e.ValorPIS).HasColumnName("valor_pis").HasColumnType("decimal(15,2)");
        builder.Property(e => e.ValorCOFINS).HasColumnName("valor_cofins").HasColumnType("decimal(15,2)");
        builder.Property(e => e.OutrasDespesas).HasColumnName("outras_despesas").HasColumnType("decimal(15,2)");
        builder.Property(e => e.ValorTotalNFe).HasColumnName("valor_total_nfe").IsRequired().HasColumnType("decimal(15,2)");
        builder.Property(e => e.ValorTotalTributos).HasColumnName("valor_total_tributos").HasColumnType("decimal(15,2)");
        builder.Property(e => e.DataCriacao).HasColumnName("data_criacao");
        builder.Property(e => e.DataAtualizacao).HasColumnName("data_atualizacao");
        builder.Property(e => e.NFeId).HasColumnName("nfe_id");

        // Índices
        builder.HasIndex(e => e.NFeId).IsUnique();

        // Relacionamentos
        builder.HasOne(t => t.NFe)
               .WithOne(n => n.Total)
               .HasForeignKey<TotalNFeEntity>(t => t.NFeId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}