// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Logging;
// using Unimake.Business.DFe.Xml.NFe;
// using Unimake.DFe.Database.Data;
// using Unimake.DFe.Database.Models;

// namespace Unimake.DFe.Database.Services;

// /// <summary>
// /// Serviço para conversão entre objetos XML da Unimake e entidades do banco de dados
// /// </summary>
// public class NFeConversionService
// {
//     private readonly NFeDbContext _context;
//     private readonly ILogger<NFeConversionService> _logger;

//     public NFeConversionService(NFeDbContext context, ILogger<NFeConversionService> logger)
//     {
//         _context = context;
//         _logger = logger;
//     }

//     /// <summary>
//     /// Converte um objeto EnviNFe da Unimake para a entidade do banco de dados
//     /// </summary>
//     /// <param name="enviNfe">Objeto EnviNFe da Unimake</param>
//     /// <returns>Entidade EnviNFeEntity</returns>
//     public async Task<EnviNFeEntity> ConvertAndSaveEnviNFeAsync(EnviNFe enviNfe)
//     {
//         try
//         {
//             _logger.LogInformation("Iniciando conversão do lote {IdLote}", enviNfe.IdLote);

//             // Verificar se o lote já existe
//             var existingLote = await _context.EnviNFes
//                 .FirstOrDefaultAsync(e => e.IdLote == enviNfe.IdLote);

//             if (existingLote != null)
//             {
//                 _logger.LogWarning("Lote {IdLote} já existe no banco de dados", enviNfe.IdLote);
//                 return existingLote;
//             }

//             // Criar nova entidade EnviNFe
//             var enviNfeEntity = new EnviNFeEntity
//             {
//                 Versao = enviNfe.Versao,
//                 IdLote = enviNfe.IdLote,
//                 IndSinc = (int)enviNfe.IndSinc,
//                 Status = "Processando"
//             };

//             _context.EnviNFes.Add(enviNfeEntity);
//             await _context.SaveChangesAsync();

//             // Processar NFes se existirem
//             if (enviNfe.NFe?.Any() == true)
//             {
//                 foreach (var nfe in enviNfe.NFe)
//                 {
//                     await ConvertAndSaveNFeAsync(nfe, enviNfeEntity.Id);
//                 }
//             }

//             enviNfeEntity.Status = "Processado";
//             enviNfeEntity.DataAtualizacao = DateTime.Now;
//             await _context.SaveChangesAsync();

//             _logger.LogInformation("Lote {IdLote} convertido e salvo com sucesso", enviNfe.IdLote);
//             return enviNfeEntity;
//         }
//         catch (Exception ex)
//         {
//             _logger.LogError(ex, "Erro ao converter e salvar lote {IdLote}", enviNfe.IdLote);
//             throw;
//         }
//     }

//     /// <summary>
//     /// Converte uma NFe da Unimake para a entidade do banco de dados
//     /// </summary>
//     /// <param name="nfe">Objeto NFe da Unimake</param>
//     /// <param name="enviNfeId">ID do lote pai</param>
//     /// <returns>Entidade NFeEntity</returns>
//     public async Task<NFeEntity> ConvertAndSaveNFeAsync(NFe nfe, int enviNfeId)
//     {
//         try
//         {
//             if (nfe.InfNFe?.Any() != true)
//             {
//                 throw new ArgumentException("NFe deve conter pelo menos uma InfNFe");
//             }

//             var infNfe = nfe.InfNFe.First(); // Pegar a primeira (normalmente é única)

//             _logger.LogInformation("Convertendo NFe número {NumeroNFe}", infNfe.Ide?.NNF);

//             // Verificar se a NFe já existe pela chave de acesso
//             var chaveAcesso = infNfe.Chave;
//             var existingNFe = await _context.NFes
//                 .FirstOrDefaultAsync(n => n.ChaveAcesso == chaveAcesso);

//             if (existingNFe != null)
//             {
//                 _logger.LogWarning("NFe com chave {ChaveAcesso} já existe", chaveAcesso);
//                 return existingNFe;
//             }

//             // Converter e salvar emitente
//             EmitenteEntity? emitenteEntity = null;
//             if (infNfe.Emit != null)
//             {
//                 emitenteEntity = await ConvertAndSaveEmitenteAsync(infNfe.Emit);
//             }

//             // Converter e salvar destinatário
//             DestinatarioEntity? destinatarioEntity = null;
//             if (infNfe.Dest != null)
//             {
//                 destinatarioEntity = await ConvertAndSaveDestinatarioAsync(infNfe.Dest);
//             }

//             // Criar entidade NFe
//             var nfeEntity = new NFeEntity
//             {
//                 ChaveAcesso = chaveAcesso,
//                 NumeroNFe = infNfe.Ide?.NNF ?? 0,
//                 Serie = infNfe.Ide?.Serie ?? 0,
//                 Modelo = (int)(infNfe.Ide?.Mod ?? 0),
//                 DataEmissao = infNfe.Ide?.DhEmi ?? DateTimeOffset.Now,
//                 DataSaidaEntrada = infNfe.Ide?.DhSaiEnt > DateTime.MinValue ? infNfe.Ide?.DhSaiEnt : null,
//                 TipoOperacao = (int)(infNfe.Ide?.TpNF ?? 0),
//                 CodigoUF = (int)(infNfe.Ide?.CUF ?? 0),
//                 TipoEmissao = (int)(infNfe.Ide?.TpEmis ?? 0),
//                 TipoAmbiente = (int)(infNfe.Ide?.TpAmb ?? 0),
//                 FinalidadeNFe = (int)(infNfe.Ide?.FinNFe ?? 0),
//                 IndicadorConsumidorFinal = (int)(infNfe.Ide?.IndFinal ?? 0),
//                 IndicadorPresenca = (int)(infNfe.Ide?.IndPres ?? 0),
//                 NaturezaOperacao = infNfe.Ide?.NatOp,
//                 XmlCompleto = nfe.GerarXML()?.OuterXml,
//                 Status = "Importado",
//                 EnviNFeId = enviNfeId,
//                 EmitenteId = emitenteEntity?.Id,
//                 DestinatarioId = destinatarioEntity?.Id
//             };

//             _context.NFes.Add(nfeEntity);
//             await _context.SaveChangesAsync();

//             // Converter e salvar itens
//             if (infNfe.Det?.Any() == true)
//             {
//                 foreach (var det in infNfe.Det)
//                 {
//                     await ConvertAndSaveItemAsync(det, nfeEntity.Id);
//                 }
//             }

//             // Converter e salvar totais
//             if (infNfe.Total != null)
//             {
//                 await ConvertAndSaveTotalAsync(infNfe.Total, nfeEntity.Id);
//             }

//             _logger.LogInformation("NFe {NumeroNFe} convertida e salva com sucesso", infNfe.Ide?.NNF);
//             return nfeEntity;
//         }
//         catch (Exception ex)
//         {
//             _logger.LogError(ex, "Erro ao converter NFe");
//             throw;
//         }
//     }

//     private async Task<EmitenteEntity> ConvertAndSaveEmitenteAsync(Emit emit)
//     {
//         var documento = emit.CNPJ ?? emit.CPF;
//         if (string.IsNullOrEmpty(documento))
//         {
//             throw new ArgumentException("Emitente deve ter CNPJ ou CPF");
//         }

//         // Verificar se o emitente já existe
//         var existingEmitente = await _context.Emitentes
//             .FirstOrDefaultAsync(e => e.CNPJ == emit.CNPJ || e.CPF == emit.CPF);

//         if (existingEmitente != null)
//         {
//             return existingEmitente;
//         }

//         var emitenteEntity = new EmitenteEntity
//         {
//             CNPJ = emit.CNPJ,
//             CPF = emit.CPF,
//             RazaoSocial = emit.XNome ?? "Não informado",
//             NomeFantasia = emit.XFant,
//             InscricaoEstadual = emit.IE,
//             InscricaoMunicipal = emit.IM,
//             CNAE = emit.CNAE,
//             CRT = (int?)emit.CRT,
//             Logradouro = emit.EnderEmit?.XLgr,
//             Numero = emit.EnderEmit?.Nro,
//             Complemento = emit.EnderEmit?.XCpl,
//             Bairro = emit.EnderEmit?.XBairro,
//             CodigoMunicipio = emit.EnderEmit?.CMun,
//             NomeMunicipio = emit.EnderEmit?.XMun,
//             UF = emit.EnderEmit?.UF.ToString(),
//             CEP = emit.EnderEmit?.CEP,
//             CodigoPais = emit.EnderEmit?.CPais,
//             NomePais = emit.EnderEmit?.XPais,
//             Telefone = emit.EnderEmit?.Fone
//         };

//         _context.Emitentes.Add(emitenteEntity);
//         await _context.SaveChangesAsync();

//         return emitenteEntity;
//     }

//     private async Task<DestinatarioEntity?> ConvertAndSaveDestinatarioAsync(Dest dest)
//     {
//         // Destinatário pode ser nulo em NFCe
//         if (dest == null)
//             return null;

//         var documento = dest.CNPJ ?? dest.CPF ?? dest.IdEstrangeiro;
        
//         // Verificar se o destinatário já existe
//         DestinatarioEntity? existingDestinatario = null;
//         if (!string.IsNullOrEmpty(documento))
//         {
//             existingDestinatario = await _context.Destinatarios
//                 .FirstOrDefaultAsync(d => d.CNPJ == dest.CNPJ || 
//                                          d.CPF == dest.CPF || 
//                                          d.IdEstrangeiro == dest.IdEstrangeiro);
//         }

//         if (existingDestinatario != null)
//         {
//             return existingDestinatario;
//         }

//         var destinatarioEntity = new DestinatarioEntity
//         {
//             CNPJ = dest.CNPJ,
//             CPF = dest.CPF,
//             IdEstrangeiro = dest.IdEstrangeiro,
//             RazaoSocial = dest.XNome,
//             IndicadorIE = (int?)dest.IndIEDest,
//             InscricaoEstadual = dest.IE,
//             ISUF = dest.ISUF,
//             InscricaoMunicipal = dest.IM,
//             Email = dest.Email,
//             Logradouro = dest.EnderDest?.XLgr,
//             Numero = dest.EnderDest?.Nro,
//             Complemento = dest.EnderDest?.XCpl,
//             Bairro = dest.EnderDest?.XBairro,
//             CodigoMunicipio = dest.EnderDest?.CMun,
//             NomeMunicipio = dest.EnderDest?.XMun,
//             UF = dest.EnderDest?.UF.ToString(),
//             CEP = dest.EnderDest?.CEP,
//             CodigoPais = dest.EnderDest?.CPais,
//             NomePais = dest.EnderDest?.XPais,
//             Telefone = dest.EnderDest?.Fone
//         };

//         _context.Destinatarios.Add(destinatarioEntity);
//         await _context.SaveChangesAsync();

//         return destinatarioEntity;
//     }

//     private async Task<ItemNFeEntity> ConvertAndSaveItemAsync(Det det, int nfeId)
//     {
//         var itemEntity = new ItemNFeEntity
//         {
//             NumeroItem = det.NItem,
//             CodigoProduto = det.Prod?.CProd ?? "N/A",
//             GTIN = det.Prod?.CEAN,
//             Descricao = det.Prod?.XProd ?? "N/A",
//             NCM = det.Prod?.NCM,
//             NVE = det.Prod?.NVE?.FirstOrDefault(),
//             CEST = det.Prod?.CEST,
//             IndEscala = det.Prod?.IndEscala?.ToString(),
//             CNPJFab = det.Prod?.CNPJFab,
//             CBenef = det.Prod?.CBenef,
//             EXTIPI = det.Prod?.EXTIPI,
//             CFOP = det.Prod?.CFOP ?? "0000",
//             UnidadeComercial = det.Prod?.UCom ?? "UN",
//             QuantidadeComercial = det.Prod?.QCom ?? 0,
//             ValorUnitarioComercial = det.Prod?.VUnCom ?? 0,
//             ValorTotal = det.Prod?.VProd ?? 0,
//             GTINTributavel = det.Prod?.CEANTrib,
//             UnidadeTributavel = det.Prod?.UTrib,
//             QuantidadeTributavel = det.Prod?.QTrib,
//             ValorUnitarioTributavel = det.Prod?.VUnTrib,
//             ValorFrete = det.Prod?.VFrete,
//             ValorSeguro = det.Prod?.VSeg,
//             ValorDesconto = det.Prod?.VDesc,
//             OutrasDespesas = det.Prod?.VOutro,
//             IndicadorTotal = (int?)det.Prod?.IndTot,
//             PedidoCompra = det.Prod?.XPed,
//             ItemPedidoCompra = det.Prod?.NItemPed,
//             NumeroFCI = det.Prod?.NFCI,
//             NFeId = nfeId
//         };

//         _context.ItensNFe.Add(itemEntity);
//         await _context.SaveChangesAsync();

//         return itemEntity;
//     }

//     private async Task<TotalNFeEntity> ConvertAndSaveTotalAsync(Total total, int nfeId)
//     {
//         var totalEntity = new TotalNFeEntity
//         {
//             BaseCalculoICMS = total.ICMSTot?.VBC,
//             ValorICMS = total.ICMSTot?.VICMS,
//             ValorICMSDesonerado = total.ICMSTot?.VICMSDeson,
//             BaseCalculoFCP = total.ICMSTot?.VBCFCP,
//             ValorFCP = total.ICMSTot?.VFCP,
//             BaseCalculoICMSST = total.ICMSTot?.VBCST,
//             ValorICMSST = total.ICMSTot?.VST,
//             BaseCalculoFCPST = total.ICMSTot?.VBCFCPST,
//             ValorFCPST = total.ICMSTot?.VFCPST,
//             BaseCalculoFCPSTRet = total.ICMSTot?.VBCFCPSTRet,
//             ValorFCPSTRet = total.ICMSTot?.VFCPSTRet,
//             ValorTotalProdutos = total.ICMSTot?.VProd,
//             ValorFrete = total.ICMSTot?.VFrete,
//             ValorSeguro = total.ICMSTot?.VSeg,
//             ValorDesconto = total.ICMSTot?.VDesc,
//             ValorII = total.ICMSTot?.VII,
//             ValorIPI = total.ICMSTot?.VIPI,
//             ValorIPIDevolvido = total.ICMSTot?.VIPIDevol,
//             ValorPIS = total.ICMSTot?.VPIS,
//             ValorCOFINS = total.ICMSTot?.VCOFINS,
//             OutrasDespesas = total.ICMSTot?.VOutro,
//             ValorTotalNFe = total.ICMSTot?.VNF ?? 0,
//             ValorTotalTributos = total.ICMSTot?.VTotTrib,
//             NFeId = nfeId
//         };

//         _context.TotaisNFe.Add(totalEntity);
//         await _context.SaveChangesAsync();

//         return totalEntity;
//     }
// }
