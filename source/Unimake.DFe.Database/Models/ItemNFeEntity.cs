using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unimake.DFe.Database.Models;

/// <summary>
/// Entidade dos itens da NFe
/// </summary>
[Table("item_nfe")]
public class ItemNFeEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Número do item
    /// </summary>
    public int NumeroItem { get; set; }

    /// <summary>
    /// Código do produto
    /// </summary>
    [Required]
    [StringLength(60)]
    public string CodigoProduto { get; set; } = string.Empty;

    /// <summary>
    /// GTIN (Global Trade Item Number) do produto
    /// </summary>
    [StringLength(14)]
    public string? GTIN { get; set; }

    /// <summary>
    /// Descrição do produto/serviço
    /// </summary>
    [Required]
    [StringLength(120)]
    public string Descricao { get; set; } = string.Empty;

    /// <summary>
    /// NCM (Nomenclatura Comum do Mercosul)
    /// </summary>
    [StringLength(8)]
    public string? NCM { get; set; }

    /// <summary>
    /// NVE (Nomenclatura de Valor Aduaneiro e Estatística)
    /// </summary>
    [StringLength(6)]
    public string? NVE { get; set; }

    /// <summary>
    /// CEST (Código Especificador da Substituição Tributária)
    /// </summary>
    [StringLength(7)]
    public string? CEST { get; set; }

    /// <summary>
    /// Indicador de escala relevante
    /// </summary>
    [StringLength(1)]
    public string? IndEscala { get; set; }

    /// <summary>
    /// CNPJ do fabricante da mercadoria
    /// </summary>
    [StringLength(14)]
    public string? CNPJFab { get; set; }

    /// <summary>
    /// Código de benefício fiscal
    /// </summary>
    [StringLength(10)]
    public string? CBenef { get; set; }

    /// <summary>
    /// Código EX TIPI
    /// </summary>
    [StringLength(3)]
    public string? EXTIPI { get; set; }

    /// <summary>
    /// Código fiscal de operações e prestações
    /// </summary>
    [Required]
    [StringLength(4)]
    public string CFOP { get; set; } = string.Empty;

    /// <summary>
    /// Unidade comercial
    /// </summary>
    [Required]
    [StringLength(6)]
    public string UnidadeComercial { get; set; } = string.Empty;

    /// <summary>
    /// Quantidade comercial
    /// </summary>
    [Column(TypeName = "decimal(15,4)")]
    public decimal QuantidadeComercial { get; set; }

    /// <summary>
    /// Valor unitário comercial
    /// </summary>
    [Column(TypeName = "decimal(21,10)")]
    public decimal ValorUnitarioComercial { get; set; }

    /// <summary>
    /// Valor total bruto dos produtos ou serviços
    /// </summary>
    [Column(TypeName = "decimal(15,2)")]
    public decimal ValorTotal { get; set; }

    /// <summary>
    /// GTIN da unidade tributável
    /// </summary>
    [StringLength(14)]
    public string? GTINTributavel { get; set; }

    /// <summary>
    /// Unidade tributável
    /// </summary>
    [StringLength(6)]
    public string? UnidadeTributavel { get; set; }

    /// <summary>
    /// Quantidade tributável
    /// </summary>
    [Column(TypeName = "decimal(15,4)")]
    public decimal? QuantidadeTributavel { get; set; }

    /// <summary>
    /// Valor unitário tributável
    /// </summary>
    [Column(TypeName = "decimal(21,10)")]
    public decimal? ValorUnitarioTributavel { get; set; }

    /// <summary>
    /// Valor do frete
    /// </summary>
    [Column(TypeName = "decimal(15,2)")]
    public decimal? ValorFrete { get; set; }

    /// <summary>
    /// Valor do seguro
    /// </summary>
    [Column(TypeName = "decimal(15,2)")]
    public decimal? ValorSeguro { get; set; }

    /// <summary>
    /// Valor do desconto
    /// </summary>
    [Column(TypeName = "decimal(15,2)")]
    public decimal? ValorDesconto { get; set; }

    /// <summary>
    /// Outras despesas acessórias
    /// </summary>
    [Column(TypeName = "decimal(15,2)")]
    public decimal? OutrasDespesas { get; set; }

    /// <summary>
    /// Indicador de composição do valor total
    /// </summary>
    public int? IndicadorTotal { get; set; }

    /// <summary>
    /// Pedido de compra
    /// </summary>
    [StringLength(15)]
    public string? PedidoCompra { get; set; }

    /// <summary>
    /// Item do pedido de compra
    /// </summary>
    public int? ItemPedidoCompra { get; set; }

    /// <summary>
    /// Número de controle da FCI
    /// </summary>
    [StringLength(36)]
    public string? NumeroFCI { get; set; }

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
