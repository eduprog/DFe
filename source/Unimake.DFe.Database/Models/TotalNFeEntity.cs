using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unimake.DFe.Database.Models;

/// <summary>
/// Entidade dos totais da NFe
/// </summary>
[Table("total_nfe")]
public class TotalNFeEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    // Totais dos produtos e serviços
    /// <summary>
    /// Base de cálculo do ICMS
    /// </summary>
    [Column(TypeName = "decimal(15,2)")]
    public decimal? BaseCalculoICMS { get; set; }

    /// <summary>
    /// Valor total do ICMS
    /// </summary>
    [Column(TypeName = "decimal(15,2)")]
    public decimal? ValorICMS { get; set; }

    /// <summary>
    /// Valor total do ICMS desonerado
    /// </summary>
    [Column(TypeName = "decimal(15,2)")]
    public decimal? ValorICMSDesonerado { get; set; }

    /// <summary>
    /// Base de cálculo do FCP
    /// </summary>
    [Column(TypeName = "decimal(15,2)")]
    public decimal? BaseCalculoFCP { get; set; }

    /// <summary>
    /// Valor total do FCP
    /// </summary>
    [Column(TypeName = "decimal(15,2)")]
    public decimal? ValorFCP { get; set; }

    /// <summary>
    /// Base de cálculo do ICMS ST
    /// </summary>
    [Column(TypeName = "decimal(15,2)")]
    public decimal? BaseCalculoICMSST { get; set; }

    /// <summary>
    /// Valor total do ICMS ST
    /// </summary>
    [Column(TypeName = "decimal(15,2)")]
    public decimal? ValorICMSST { get; set; }

    /// <summary>
    /// Base de cálculo do FCP ST
    /// </summary>
    [Column(TypeName = "decimal(15,2)")]
    public decimal? BaseCalculoFCPST { get; set; }

    /// <summary>
    /// Valor total do FCP ST
    /// </summary>
    [Column(TypeName = "decimal(15,2)")]
    public decimal? ValorFCPST { get; set; }

    /// <summary>
    /// Base de cálculo do FCP ST Retido
    /// </summary>
    [Column(TypeName = "decimal(15,2)")]
    public decimal? BaseCalculoFCPSTRet { get; set; }

    /// <summary>
    /// Valor total do FCP ST Retido
    /// </summary>
    [Column(TypeName = "decimal(15,2)")]
    public decimal? ValorFCPSTRet { get; set; }

    /// <summary>
    /// Valor total dos produtos e serviços
    /// </summary>
    [Column(TypeName = "decimal(15,2)")]
    public decimal? ValorTotalProdutos { get; set; }

    /// <summary>
    /// Valor total do frete
    /// </summary>
    [Column(TypeName = "decimal(15,2)")]
    public decimal? ValorFrete { get; set; }

    /// <summary>
    /// Valor total do seguro
    /// </summary>
    [Column(TypeName = "decimal(15,2)")]
    public decimal? ValorSeguro { get; set; }

    /// <summary>
    /// Valor total do desconto
    /// </summary>
    [Column(TypeName = "decimal(15,2)")]
    public decimal? ValorDesconto { get; set; }

    /// <summary>
    /// Valor total do II
    /// </summary>
    [Column(TypeName = "decimal(15,2)")]
    public decimal? ValorII { get; set; }

    /// <summary>
    /// Valor total do IPI
    /// </summary>
    [Column(TypeName = "decimal(15,2)")]
    public decimal? ValorIPI { get; set; }

    /// <summary>
    /// Valor total do IPI devolvido
    /// </summary>
    [Column(TypeName = "decimal(15,2)")]
    public decimal? ValorIPIDevolvido { get; set; }

    /// <summary>
    /// Valor do PIS
    /// </summary>
    [Column(TypeName = "decimal(15,2)")]
    public decimal? ValorPIS { get; set; }

    /// <summary>
    /// Valor da COFINS
    /// </summary>
    [Column(TypeName = "decimal(15,2)")]
    public decimal? ValorCOFINS { get; set; }

    /// <summary>
    /// Outras despesas acessórias
    /// </summary>
    [Column(TypeName = "decimal(15,2)")]
    public decimal? OutrasDespesas { get; set; }

    /// <summary>
    /// Valor total da NFe
    /// </summary>
    [Required]
    [Column(TypeName = "decimal(15,2)")]
    public decimal ValorTotalNFe { get; set; }

    /// <summary>
    /// Valor aproximado total de tributos federais
    /// </summary>
    [Column(TypeName = "decimal(15,2)")]
    public decimal? ValorTotalTributos { get; set; }

    /// <summary>
    /// Data de criação do registro
    /// </summary>
    public DateTime DataCriacao { get; set; } = DateTime.Now;

    /// <summary>
    /// Data da última atualização
    /// </summary>
    public DateTime? DataAtualizacao { get; set; }

    // Foreign Key
    [ForeignKey("NFe")]
    public int NFeId { get; set; }

    // Relacionamentos
    public virtual NFeEntity NFe { get; set; } = null!;
}
