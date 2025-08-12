using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unimake.DFe.Database.Models;

/// <summary>
/// Entidade do emitente da NFe
/// </summary>
[Table("emitente")]
public class EmitenteEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// CNPJ do emitente
    /// </summary>
    [StringLength(14)]
    public string? CNPJ { get; set; }

    /// <summary>
    /// CPF do emitente
    /// </summary>
    [StringLength(11)]
    public string? CPF { get; set; }

    /// <summary>
    /// Razão Social ou Nome do emitente
    /// </summary>
    [Required]
    [StringLength(60)]
    public string RazaoSocial { get; set; } = string.Empty;

    /// <summary>
    /// Nome fantasia
    /// </summary>
    [StringLength(60)]
    public string? NomeFantasia { get; set; }

    /// <summary>
    /// Inscrição estadual do emitente
    /// </summary>
    [StringLength(14)]
    public string? InscricaoEstadual { get; set; }

    /// <summary>
    /// Inscrição Municipal
    /// </summary>
    [StringLength(15)]
    public string? InscricaoMunicipal { get; set; }

    /// <summary>
    /// CNAE Fiscal
    /// </summary>
    [StringLength(7)]
    public string? CNAE { get; set; }

    /// <summary>
    /// Código de Regime Tributário
    /// </summary>
    public int? CRT { get; set; }

    // Endereço
    [StringLength(60)]
    public string? Logradouro { get; set; }

    [StringLength(60)]
    public string? Numero { get; set; }

    [StringLength(60)]
    public string? Complemento { get; set; }

    [StringLength(60)]
    public string? Bairro { get; set; }

    public int? CodigoMunicipio { get; set; }

    [StringLength(60)]
    public string? NomeMunicipio { get; set; }

    [StringLength(2)]
    public string? UF { get; set; }

    [StringLength(8)]
    public string? CEP { get; set; }

    public int? CodigoPais { get; set; } = 1058;

    [StringLength(60)]
    public string? NomePais { get; set; } = "BRASIL";

    [StringLength(14)]
    public string? Telefone { get; set; }

    /// <summary>
    /// Data de criação do registro
    /// </summary>
    public DateTime DataCriacao { get; set; } = DateTime.Now;

    /// <summary>
    /// Data da última atualização
    /// </summary>
    public DateTime? DataAtualizacao { get; set; }

    // Relacionamentos
    public virtual ICollection<NFeEntity> NFesEmitidas { get; set; } = new List<NFeEntity>();
}
