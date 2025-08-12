using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unimake.DFe.Database.Models;

/// <summary>
/// Entidade principal do lote da NFe/NFCe
/// </summary>
[Table("EnviNFe")]
public class EnviNFeEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Versão do schema do XML do lote da NFe/NFCe
    /// </summary>
    [StringLength(10)]
    public string? Versao { get; set; }

    /// <summary>
    /// Número do lote
    /// </summary>
    [Required]
    [StringLength(15)]
    public string IdLote { get; set; } = string.Empty;

    /// <summary>
    /// Indicador de processamento síncrono (0-Não, 1-Sim)
    /// </summary>
    public int IndSinc { get; set; }

    /// <summary>
    /// Data de criação do registro
    /// </summary>
    public DateTime DataCriacao { get; set; } = DateTime.Now;

    /// <summary>
    /// Data da última atualização
    /// </summary>
    public DateTime? DataAtualizacao { get; set; }

    /// <summary>
    /// Status do processamento
    /// </summary>
    [StringLength(50)]
    public string? Status { get; set; }

    /// <summary>
    /// Observações/Log
    /// </summary>
    [StringLength(1000)]
    public string? Observacoes { get; set; }

    // Relacionamentos
    public virtual ICollection<NFeEntity> NFes { get; set; } = new List<NFeEntity>();
}
