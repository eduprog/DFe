using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Unimake.Business.DFe.Xml.NFe.Examples
{
    /// <summary>
    /// Exemplo de DTO que seria gerado automaticamente para EnviNFe
    /// Este arquivo é apenas demonstrativo - será gerado automaticamente pelo Source Generator
    /// </summary>
    public partial class EnviNFeIncluirRequest
    {
        /// <summary>
        /// Versão do schema do XML do lote da NFe/NFCe
        /// </summary>
        [Required]
        [JsonPropertyName("versao")]
        public string Versao { get; set; }

        /// <summary>
        /// Número do lote
        /// </summary>
        [Required]
        [JsonPropertyName("idLote")]
        public string IdLote { get; set; }

        /// <summary>
        /// Indicador de processamento síncrono
        /// </summary>
        [JsonPropertyName("indSinc")]
        public SimNao IndSinc { get; set; }

        /// <summary>
        /// NFe/NFCe
        /// </summary>
        [Required]
        [JsonPropertyName("nfe")]
        public List<NFe> NFe { get; set; } = new();
    }

    /// <summary>
    /// DTO para atualização de EnviNFe
    /// </summary>
    public partial class EnviNFeAtualizarRequest
    {
        /// <summary>
        /// Versão do schema do XML do lote da NFe/NFCe
        /// </summary>
        [JsonPropertyName("versao")]
        public string? Versao { get; set; }

        /// <summary>
        /// Número do lote
        /// </summary>
        [Required]
        [JsonPropertyName("idLote")]
        public string IdLote { get; set; }

        /// <summary>
        /// Indicador de processamento síncrono
        /// </summary>
        [JsonPropertyName("indSinc")]
        public SimNao? IndSinc { get; set; }

        /// <summary>
        /// NFe/NFCe
        /// </summary>
        [JsonPropertyName("nfe")]
        public List<NFe>? NFe { get; set; }
    }

    /// <summary>
    /// DTO para consulta de EnviNFe
    /// </summary>
    public partial class EnviNFeConsultarRequest
    {
        /// <summary>
        /// Número do lote
        /// </summary>
        [Required]
        [JsonPropertyName("idLote")]
        public string IdLote { get; set; }

        /// <summary>
        /// Filtros de consulta (opcional)
        /// </summary>
        [JsonPropertyName("filtros")]
        public ConsultaFiltros? Filtros { get; set; }
    }

    /// <summary>
    /// Filtros para consulta
    /// </summary>
    public class ConsultaFiltros
    {
        [JsonPropertyName("dataInicio")]
        public DateTime? DataInicio { get; set; }

        [JsonPropertyName("dataFim")]
        public DateTime? DataFim { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }
    }
}
