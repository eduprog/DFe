using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unimake.DFe.Database.Models;

/// <summary>
/// Entidade do destinatário da NFe
/// </summary>
[Table("destinatario")]
public class DestinatarioEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// CNPJ do destinatário
    /// </summary>
    [StringLength(14)]
    public string? CNPJ { get; set; }

    /// <summary>
    /// CPF do destinatário
    /// </summary>
    [StringLength(11)]
    public string? CPF { get; set; }

    /// <summary>
    /// Identificador do destinatário estrangeiro
    /// </summary>
    [StringLength(20)]
    public string? IdEstrangeiro { get; set; }

    /// <summary>
    /// Razão Social ou nome do destinatário
    /// </summary>
    [StringLength(60)]
    public string? RazaoSocial { get; set; }

    /// <summary>
    /// Indicador da IE do destinatário
    /// </summary>
    public int? IndicadorIE { get; set; }

    /// <summary>
    /// Inscrição Estadual
    /// </summary>
    [StringLength(14)]
    public string? InscricaoEstadual { get; set; }

    /// <summary>
    /// Inscrição na SUFRAMA
    /// </summary>
    [StringLength(9)]
    public string? ISUF { get; set; }

    /// <summary>
    /// Inscrição Municipal
    /// </summary>
    [StringLength(15)]
    public string? InscricaoMunicipal { get; set; }

    /// <summary>
    /// E-mail do destinatário
    /// </summary>
    [StringLength(60)]
    public string? Email { get; set; }

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
    public virtual ICollection<NFeEntity> NFesRecebidas { get; set; } = new List<NFeEntity>();
}
