using Unimake.Business.DFe.SourceGenerators.Attributes;
using Unimake.Business.DFe.Xml.NFe;

namespace Unimake.Business.DFe.SourceGenerators.Wrappers
{
    /// <summary>
    /// Classe wrapper para EnviNFe que permite configurar a geração de DTOs
    /// sem modificar a classe original
    /// </summary>
    [SourceGeneratorConfig(
        typeof(EnviNFe), 
        DtoType.IncluirRequest, 
        DtoType.AtualizarRequest, 
        DtoType.ConsultarRequest,
        CustomNamespace = "Unimake.Business.DFe.Dtos.NFe")]
    [PropertyConfig("Versao", RequiredForTypes = new[] { DtoType.IncluirRequest })]
    [PropertyConfig("IdLote", RequiredForTypes = new[] { DtoType.IncluirRequest, DtoType.AtualizarRequest, DtoType.ConsultarRequest })]
    [PropertyConfig("NFe", CustomName = "NotasFiscais")]
    public class EnviNFeSourceGenerator : EnviNFe
    {
        // Esta classe serve apenas como configuração para o Source Generator
        // Não precisa implementar nada, apenas herdar da classe base
        
        // Opcionalmente, você pode adicionar métodos auxiliares específicos para DTOs
        
        /// <summary>
        /// Método auxiliar para converter do DTO de inclusão para a classe XML
        /// (será gerado automaticamente pelo Source Generator)
        /// </summary>
        // public static EnviNFe FromIncluirRequest(EnviNFeIncluirRequest dto) { ... }
        
        /// <summary>
        /// Método auxiliar para converter da classe XML para o DTO de inclusão
        /// (será gerado automaticamente pelo Source Generator)
        /// </summary>
        // public EnviNFeIncluirRequest ToIncluirRequest() { ... }
    }

    /// <summary>
    /// Wrapper para NFe individual
    /// </summary>
    [SourceGeneratorConfig(
        typeof(NFe), 
        DtoType.IncluirRequest, 
        DtoType.AtualizarRequest,
        CustomNamespace = "Unimake.Business.DFe.Dtos.NFe")]
    [PropertyConfig("InfNFe", RequiredForTypes = new[] { DtoType.IncluirRequest })]
    [PropertyConfig("Signature", ExcludeFromTypes = new[] { DtoType.IncluirRequest, DtoType.AtualizarRequest })]
    public class NFeSourceGenerator : NFe
    {
        // Configuração para geração de DTOs da NFe
    }

    /// <summary>
    /// Wrapper para InfNFe
    /// </summary>
    [SourceGeneratorConfig(
        typeof(InfNFe), 
        DtoType.IncluirRequest, 
        DtoType.AtualizarRequest,
        CustomNamespace = "Unimake.Business.DFe.Dtos.NFe")]
    [PropertyConfig("Ide", RequiredForTypes = new[] { DtoType.IncluirRequest })]
    [PropertyConfig("Emit", RequiredForTypes = new[] { DtoType.IncluirRequest })]
    [PropertyConfig("Det", RequiredForTypes = new[] { DtoType.IncluirRequest })]
    [PropertyConfig("Total", RequiredForTypes = new[] { DtoType.IncluirRequest })]
    [PropertyConfig("Chave", ExcludeFromTypes = new[] { DtoType.IncluirRequest, DtoType.AtualizarRequest })] // Calculado automaticamente
    [PropertyConfig("Id", ExcludeFromTypes = new[] { DtoType.IncluirRequest, DtoType.AtualizarRequest })] // Calculado automaticamente
    public class InfNFeSourceGenerator : InfNFe
    {
        // Configuração para geração de DTOs da InfNFe
    }
}
