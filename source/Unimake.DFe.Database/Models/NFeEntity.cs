using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unimake.DFe.Database.Models;

/// <summary>
/// Entidade da NFe/NFCe
/// </summary>
[Table("nfe")]
public class NFeEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Chave de acesso da NFe (44 dígitos)
    /// </summary>
    [Required]
    [StringLength(44)]
    public string ChaveAcesso { get; set; } = string.Empty;

    /// <summary>
    /// Número da NFe
    /// </summary>
    public int NumeroNFe { get; set; }

    /// <summary>
    /// Série da NFe
    /// </summary>
    public int Serie { get; set; }

    /// <summary>
    /// Modelo (55-NFe, 65-NFCe)
    /// </summary>
    public int Modelo { get; set; }

    /// <summary>
    /// Data de emissão
    /// </summary>
    public DateTimeOffset DataEmissao { get; set; }

    /// <summary>
    /// Data de saída/entrada
    /// </summary>
    public DateTimeOffset? DataSaidaEntrada { get; set; }

    /// <summary>
    /// Tipo de operação (0-Entrada, 1-Saída)
    /// </summary>
    public int TipoOperacao { get; set; }

    /// <summary>
    /// Código da UF do emitente
    /// </summary>
    public int CodigoUF { get; set; }

    /// <summary>
    /// Tipo de emissão
    /// </summary>
    public int TipoEmissao { get; set; }

    /// <summary>
    /// Tipo de ambiente (1-Produção, 2-Homologação)
    /// </summary>
    public int TipoAmbiente { get; set; }

    /// <summary>
    /// Finalidade da NFe
    /// </summary>
    public int FinalidadeNFe { get; set; }

    /// <summary>
    /// Indicador de consumidor final
    /// </summary>
    public int IndicadorConsumidorFinal { get; set; }

    /// <summary>
    /// Indicador de presença
    /// </summary>
    public int IndicadorPresenca { get; set; }

    /// <summary>
    /// Natureza da operação
    /// </summary>
    [StringLength(60)]
    public string? NaturezaOperacao { get; set; }

    /// <summary>
    /// XML da NFe completo (para auditoria)
    /// </summary>
    public string? XmlCompleto { get; set; }

    /// <summary>
    /// Status da NFe
    /// </summary>
    [StringLength(50)]
    public string? Status { get; set; }

    /// <summary>
    /// Data de criação do registro
    /// </summary>
    public DateTime DataCriacao { get; set; } = DateTime.Now;

    /// <summary>
    /// Data da última atualização
    /// </summary>
    public DateTime? DataAtualizacao { get; set; }

    // Foreign Keys
    [ForeignKey("EnviNFe")]
    public int EnviNFeId { get; set; }

    [ForeignKey("Emitente")]
    public int? EmitenteId { get; set; }

    [ForeignKey("Destinatario")]
    public int? DestinatarioId { get; set; }

    // Relacionamentos
    public virtual EnviNFeEntity EnviNFe { get; set; } = null!;
    public virtual EmitenteEntity? Emitente { get; set; }
    public virtual DestinatarioEntity? Destinatario { get; set; }
    public virtual ICollection<ItemNFeEntity> Itens { get; set; } = new List<ItemNFeEntity>();
    public virtual TotalNFeEntity? Total { get; set; }
}
