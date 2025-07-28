using Unimake.Business.DFe.SourceGenerators.Attributes;
using Unimake.Business.DFe.Xml.NFe;

namespace Unimake.Business.DFe.SourceGenerators.Wrappers.NFe
{
    /// <summary>
    /// Classe wrapper para EnviNFe que permite configurar a geração de DTOs
    /// sem modificar a classe original.
    /// 
    /// Esta classe herda de EnviNFe e adiciona apenas os atributos necessários
    /// para configurar o Source Generator.
    /// </summary>
    [SourceGeneratorConfig(
        typeof(EnviNFe), 
        DtoType.IncluirRequest, 
        DtoType.AtualizarRequest, 
        DtoType.ConsultarRequest,
        CustomNamespace = "Unimake.Business.DFe.Dtos.NFe")]
    [PropertyConfig("Versao", RequiredForTypes = new[] { DtoType.IncluirRequest })]
    [PropertyConfig("IdLote", RequiredForTypes = new[] { DtoType.IncluirRequest, DtoType.AtualizarRequest, DtoType.ConsultarRequest })]
    [PropertyConfig("NFe", CustomName = "NotasFiscais", RequiredForTypes = new[] { DtoType.IncluirRequest })]
    public class EnviNFeWrapper : EnviNFe
    {
        // Esta classe serve apenas como configuração para o Source Generator
        // Não precisa implementar nada adicional, apenas herdar da classe base
        
        // O Source Generator irá:
        // 1. Analisar a classe base EnviNFe
        // 2. Aplicar as configurações dos atributos PropertyConfig
        // 3. Gerar os DTOs: EnviNFeIncluirRequest, EnviNFeAtualizarRequest, EnviNFeConsultarRequest
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
    [PropertyConfig("InfNFeSupl", ExcludeFromTypes = new[] { DtoType.IncluirRequest })] // Geralmente não usado em criação
    [PropertyConfig("Signature", ExcludeFromTypes = new[] { DtoType.IncluirRequest, DtoType.AtualizarRequest })] // Assinatura é gerada depois
    public class NFeWrapper : NFe
    {
        // Configuração para DTOs da NFe individual
    }

    /// <summary>
    /// Wrapper para InfNFe (Informações da NFe)
    /// </summary>
    [SourceGeneratorConfig(
        typeof(InfNFe), 
        DtoType.IncluirRequest, 
        DtoType.AtualizarRequest,
        CustomNamespace = "Unimake.Business.DFe.Dtos.NFe")]
    // Campos obrigatórios para inclusão
    [PropertyConfig("Versao", RequiredForTypes = new[] { DtoType.IncluirRequest })]
    [PropertyConfig("Ide", RequiredForTypes = new[] { DtoType.IncluirRequest })]
    [PropertyConfig("Emit", RequiredForTypes = new[] { DtoType.IncluirRequest })]
    [PropertyConfig("Det", RequiredForTypes = new[] { DtoType.IncluirRequest })]
    [PropertyConfig("Total", RequiredForTypes = new[] { DtoType.IncluirRequest })]
    
    // Campos calculados/gerados automaticamente - excluir de inclusão
    [PropertyConfig("Chave", ExcludeFromTypes = new[] { DtoType.IncluirRequest, DtoType.AtualizarRequest })]
    [PropertyConfig("Id", ExcludeFromTypes = new[] { DtoType.IncluirRequest, DtoType.AtualizarRequest })]
    
    // Campos opcionais
    [PropertyConfig("Dest")] // Destinatário - opcional em alguns casos
    [PropertyConfig("Retirada", ExcludeFromTypes = new[] { DtoType.IncluirRequest })] // Raramente usado
    [PropertyConfig("Entrega", ExcludeFromTypes = new[] { DtoType.IncluirRequest })] // Raramente usado
    [PropertyConfig("AutXML", ExcludeFromTypes = new[] { DtoType.IncluirRequest })] // Autorização posterior
    [PropertyConfig("Transp")] // Transporte - às vezes obrigatório
    [PropertyConfig("Cobr")] // Cobrança - opcional
    [PropertyConfig("Pag", RequiredForTypes = new[] { DtoType.IncluirRequest })] // Pagamento obrigatório para NFCe
    [PropertyConfig("InfAdic")] // Informações adicionais - opcional
    [PropertyConfig("Exporta", ExcludeFromTypes = new[] { DtoType.IncluirRequest })] // Só para exportação
    [PropertyConfig("Compra", ExcludeFromTypes = new[] { DtoType.IncluirRequest })] // Informações de compra governamental
    [PropertyConfig("Cana", ExcludeFromTypes = new[] { DtoType.IncluirRequest })] // Só para cana de açúcar
    [PropertyConfig("InfRespTec")] // Responsável técnico - opcional
    [PropertyConfig("InfSolicNFF", ExcludeFromTypes = new[] { DtoType.IncluirRequest })] // NFF específica
    [PropertyConfig("Agropecuario", ExcludeFromTypes = new[] { DtoType.IncluirRequest })] // Produtos agropecuários específicos
    public class InfNFeWrapper : InfNFe
    {
        // Configuração para DTOs das informações da NFe
        // Esta é a classe mais complexa pois InfNFe tem muitos campos
    }

    /// <summary>
    /// Wrapper para Ide (Identificação da NFe)
    /// </summary>
    [SourceGeneratorConfig(
        typeof(Ide), 
        DtoType.IncluirRequest, 
        DtoType.AtualizarRequest,
        CustomNamespace = "Unimake.Business.DFe.Dtos.NFe")]
    // Campos obrigatórios básicos
    [PropertyConfig("CUF", RequiredForTypes = new[] { DtoType.IncluirRequest }, CustomName = "CodigoUF")]
    [PropertyConfig("NatOp", RequiredForTypes = new[] { DtoType.IncluirRequest }, CustomName = "NaturezaOperacao")]
    [PropertyConfig("Mod", RequiredForTypes = new[] { DtoType.IncluirRequest }, CustomName = "Modelo")]
    [PropertyConfig("Serie", RequiredForTypes = new[] { DtoType.IncluirRequest })]
    [PropertyConfig("NNF", RequiredForTypes = new[] { DtoType.IncluirRequest }, CustomName = "NumeroNF")]
    [PropertyConfig("DhEmi", RequiredForTypes = new[] { DtoType.IncluirRequest }, CustomName = "DataEmissao")]
    [PropertyConfig("TpNF", RequiredForTypes = new[] { DtoType.IncluirRequest }, CustomName = "TipoNF")]
    [PropertyConfig("IdDest", RequiredForTypes = new[] { DtoType.IncluirRequest }, CustomName = "DestinoOperacao")]
    [PropertyConfig("CMunFG", RequiredForTypes = new[] { DtoType.IncluirRequest }, CustomName = "CodigoMunicipioFatoGerador")]
    [PropertyConfig("TpImp", RequiredForTypes = new[] { DtoType.IncluirRequest }, CustomName = "TipoImpressao")]
    [PropertyConfig("TpEmis", RequiredForTypes = new[] { DtoType.IncluirRequest }, CustomName = "TipoEmissao")]
    [PropertyConfig("TpAmb", RequiredForTypes = new[] { DtoType.IncluirRequest }, CustomName = "TipoAmbiente")]
    [PropertyConfig("FinNFe", RequiredForTypes = new[] { DtoType.IncluirRequest }, CustomName = "FinalidadeNFe")]
    [PropertyConfig("IndFinal", RequiredForTypes = new[] { DtoType.IncluirRequest }, CustomName = "ConsumidorFinal")]
    [PropertyConfig("IndPres", RequiredForTypes = new[] { DtoType.IncluirRequest }, CustomName = "PresencaComprador")]
    [PropertyConfig("ProcEmi", RequiredForTypes = new[] { DtoType.IncluirRequest }, CustomName = "ProcessoEmissao")]
    [PropertyConfig("VerProc", RequiredForTypes = new[] { DtoType.IncluirRequest }, CustomName = "VersaoProcesso")]
    
    // Campos calculados/gerados - excluir
    [PropertyConfig("CNF", ExcludeFromTypes = new[] { DtoType.IncluirRequest, DtoType.AtualizarRequest })] // Código numérico gerado
    [PropertyConfig("CDV", ExcludeFromTypes = new[] { DtoType.IncluirRequest, DtoType.AtualizarRequest })] // Dígito verificador calculado
    
    // Campos opcionais/específicos
    [PropertyConfig("DhSaiEnt", CustomName = "DataSaidaEntrada")] // Opcional
    [PropertyConfig("CMunFGIBS", CustomName = "CodigoMunicipioIBS")] // Novo campo IBS/CBS
    [PropertyConfig("TpNFDebito", CustomName = "TipoNotaDebito")] // Específico
    [PropertyConfig("TpNFCredito", CustomName = "TipoNotaCredito")] // Específico
    [PropertyConfig("IndIntermed", CustomName = "IndicadorIntermediario")] // Marketplace
    
    // Contingência - raramente usado na criação
    [PropertyConfig("DhCont", ExcludeFromTypes = new[] { DtoType.IncluirRequest }, CustomName = "DataContingencia")]
    [PropertyConfig("XJust", ExcludeFromTypes = new[] { DtoType.IncluirRequest }, CustomName = "JustificativaContingencia")]
    
    // Referências e compras governamentais - específicos
    [PropertyConfig("NFref", CustomName = "NotasReferenciadas")]
    [PropertyConfig("GCompraGov", ExcludeFromTypes = new[] { DtoType.IncluirRequest }, CustomName = "CompraGovernamental")]
    [PropertyConfig("GPagAntecipado", ExcludeFromTypes = new[] { DtoType.IncluirRequest }, CustomName = "PagamentoAntecipado")]
    public class IdeWrapper : Ide
    {
        // Configuração para DTOs da identificação da NFe
        // Muitos campos com nomes mais amigáveis para API
    }
}
