// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Logging;
// using Unimake.Business.DFe.Xml.NFe;
// using Unimake.DFe.Database.Data;
// using Unimake.DFe.Database.Services;

// namespace Unimake.DFe.Database.Examples;

// /// <summary>
// /// Exemplos práticos de uso do sistema
// /// </summary>
// public static class UsageExamples
// {
//     /// <summary>
//     /// Exemplo de criação de NFe simples e persistência no banco
//     /// </summary>
//     public static async Task ExemploNFeSimples(IServiceProvider serviceProvider)
//     {
//         var context = serviceProvider.GetRequiredService<NFeDbContext>();
//         var conversionService = serviceProvider.GetRequiredService<NFeConversionService>();
//         var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

//         logger.LogInformation("🔧 Criando exemplo de NFe simples...");

//         try
//         {
//             // Criar objeto EnviNFe da Unimake
//             var enviNfe = new EnviNFe
//             {
//                 Versao = "4.00",
//                 IdLote = DateTime.Now.ToString("yyyyMMddHHmmss"),
//                 IndSinc = Business.DFe.Servicos.SimNao.Sim,
//                 NFe = new List<NFe>()
//             };

//             // Criar NFe
//             var nfe = new NFe
//             {
//                 InfNFe = new List<InfNFe>()
//             };

//             var infNfe = new InfNFe
//             {
//                 Versao = "4.00",
//                 Ide = new Ide
//                 {
//                     CUF = Business.DFe.Servicos.UFBrasil.SP,
//                     NatOp = "Venda de produtos",
//                     Mod = Business.DFe.Servicos.ModeloDFe.NFe,
//                     Serie = 1,
//                     NNF = new Random().Next(1, 999999),
//                     DhEmi = DateTimeOffset.Now,
//                     TpNF = Business.DFe.Servicos.TipoOperacao.Saida,
//                     IdDest = Business.DFe.Servicos.DestinoOperacao.OperacaoInterna,
//                     CMunFG = 3550308, // São Paulo
//                     TpImp = Business.DFe.Servicos.FormatoImpressaoDANFE.NormalRetrato,
//                     TpEmis = Business.DFe.Servicos.TipoEmissao.Normal,
//                     TpAmb = Business.DFe.Servicos.TipoAmbiente.Homologacao,
//                     FinNFe = Business.DFe.Servicos.FinalidadeNFe.Normal,
//                     IndFinal = Business.DFe.Servicos.SimNao.Sim,
//                     IndPres = Business.DFe.Servicos.IndicadorPresenca.PresencialForaEstabelecimento,
//                     ProcEmi = Business.DFe.Servicos.ProcessoEmissao.AplicativoContribuinte,
//                     VerProc = "1.0.0"
//                 },
//                 Emit = new Emit
//                 {
//                     CNPJ = "12345678000195",
//                     XNome = "EMPRESA EXEMPLO LTDA",
//                     XFant = "Empresa Exemplo",
//                     IE = "123456789",
//                     CRT = Business.DFe.Servicos.CRT.SimplesNacional,
//                     EnderEmit = new EnderEmit
//                     {
//                         XLgr = "Rua das Flores",
//                         Nro = "123",
//                         XBairro = "Centro",
//                         CMun = 3550308,
//                         XMun = "São Paulo",
//                         UF = Business.DFe.Servicos.UFBrasil.SP,
//                         CEP = "01234567"
//                     }
//                 },
//                 Dest = new Dest
//                 {
//                     CPF = "12345678901",
//                     XNome = "CLIENTE EXEMPLO",
//                     IndIEDest = Business.DFe.Servicos.IndicadorIEDestinatario.NaoContribuinte,
//                     EnderDest = new EnderDest
//                     {
//                         XLgr = "Rua do Cliente",
//                         Nro = "456",
//                         XBairro = "Vila Nova",
//                         CMun = 3550308,
//                         XMun = "São Paulo",
//                         UF = Business.DFe.Servicos.UFBrasil.SP,
//                         CEP = "09876543"
//                     }
//                 },
//                 Det = new List<Det>
//                 {
//                     new Det
//                     {
//                         NItem = 1,
//                         Prod = new Prod
//                         {
//                             CProd = "PROD001",
//                             CEAN = "",
//                             XProd = "Produto de Exemplo",
//                             NCM = "12345678",
//                             CFOP = "5102",
//                             UCom = "UN",
//                             QCom = 1,
//                             VUnCom = 100.00m,
//                             VProd = 100.00m,
//                             CEANTrib = "",
//                             UTrib = "UN",
//                             QTrib = 1,
//                             VUnTrib = 100.00m,
//                             IndTot = Business.DFe.Servicos.IndicadorTotal.ValorDoItemCompoeTotalNFe
//                         }
//                     }
//                 },
//                 Total = new Total
//                 {
//                     ICMSTot = new ICMSTot
//                     {
//                         VBC = 0,
//                         VICMS = 0,
//                         VICMSDeson = 0,
//                         VBCST = 0,
//                         VST = 0,
//                         VProd = 100.00m,
//                         VFrete = 0,
//                         VSeg = 0,
//                         VDesc = 0,
//                         VII = 0,
//                         VIPI = 0,
//                         VPIS = 0,
//                         VCOFINS = 0,
//                         VOutro = 0,
//                         VNF = 100.00m
//                     }
//                 }
//             };

//             nfe.InfNFe.Add(infNfe);
//             enviNfe.NFe.Add(nfe);

//             // Converter e salvar no banco
//             var enviNfeEntity = await conversionService.ConvertAndSaveEnviNFeAsync(enviNfe);

//             logger.LogInformation("✅ NFe exemplo criada com sucesso!");
//             logger.LogInformation("   - Lote ID: {LoteId}", enviNfeEntity.IdLote);
//             logger.LogInformation("   - NFe Número: {NumeroNFe}", infNfe.Ide.NNF);
//             logger.LogInformation("   - Chave de Acesso: {ChaveAcesso}", infNfe.Chave);

//             // Exemplo de consulta
//             await ExemploConsultas(context, logger);
//         }
//         catch (Exception ex)
//         {
//             logger.LogError(ex, "❌ Erro ao criar NFe exemplo");
//             throw;
//         }
//     }

//     /// <summary>
//     /// Exemplos de consultas no banco de dados
//     /// </summary>
//     public static async Task ExemploConsultas(NFeDbContext context, ILogger logger)
//     {
//         logger.LogInformation("🔍 Executando exemplos de consultas...");

//         // 1. Buscar todas as NFes de um período
//         var dataInicio = DateTime.Today.AddDays(-30);
//         var nfesRecentes = await context.NFes
//             .Where(n => n.DataEmissao >= dataInicio)
//             .Include(n => n.Emitente)
//             .Include(n => n.Destinatario)
//             .ToListAsync();

//         logger.LogInformation("📊 NFes dos últimos 30 dias: {Count}", nfesRecentes.Count);

//         // 2. Buscar NFes por emitente
//         var nfesPorEmitente = await context.NFes
//             .Include(n => n.Emitente)
//             .Where(n => n.Emitente!.CNPJ == "12345678000195")
//             .ToListAsync();

//         logger.LogInformation("🏢 NFes do emitente específico: {Count}", nfesPorEmitente.Count);

//         // 3. Relatório de valores por mês
//         var relatorioMensal = await context.NFes
//             .Include(n => n.Total)
//             .Where(n => n.DataEmissao.Year == DateTime.Now.Year)
//             .GroupBy(n => n.DataEmissao.Month)
//             .Select(g => new
//             {
//                 Mes = g.Key,
//                 QuantidadeNFes = g.Count(),
//                 ValorTotal = g.Sum(n => n.Total!.ValorTotalNFe)
//             })
//             .ToListAsync();

//         logger.LogInformation("📈 Relatório mensal do ano atual:");
//         foreach (var item in relatorioMensal)
//         {
//             logger.LogInformation("   Mês {Mes}: {Quantidade} NFes, Total: R$ {Valor:C}", 
//                 item.Mes, item.QuantidadeNFes, item.ValorTotal);
//         }

//         // 4. Top 10 produtos mais vendidos
//         var topProdutos = await context.ItensNFe
//             .GroupBy(i => new { i.CodigoProduto, i.Descricao })
//             .Select(g => new
//             {
//                 g.Key.CodigoProduto,
//                 g.Key.Descricao,
//                 QuantidadeVendida = g.Sum(i => i.QuantidadeComercial),
//                 ValorTotal = g.Sum(i => i.ValorTotal)
//             })
//             .OrderByDescending(p => p.QuantidadeVendida)
//             .Take(10)
//             .ToListAsync();

//         logger.LogInformation("🏆 Top 10 produtos mais vendidos:");
//         foreach (var produto in topProdutos)
//         {
//             logger.LogInformation("   {Codigo} - {Descricao}: {Quantidade} unidades, R$ {Valor:C}",
//                 produto.CodigoProduto, produto.Descricao, produto.QuantidadeVendida, produto.ValorTotal);
//         }
//     }

//     /// <summary>
//     /// Exemplo de importação de XML existente
//     /// </summary>
//     public static async Task ExemploImportacaoXML(IServiceProvider serviceProvider, string caminhoXml)
//     {
//         var conversionService = serviceProvider.GetRequiredService<NFeConversionService>();
//         var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

//         logger.LogInformation("📁 Importando XML: {Caminho}", caminhoXml);

//         try
//         {
//             if (!File.Exists(caminhoXml))
//             {
//                 logger.LogWarning("❌ Arquivo XML não encontrado: {Caminho}", caminhoXml);
//                 return;
//             }

//             // Carregar XML usando a biblioteca da Unimake
//             var enviNfe = new EnviNFe();
//             enviNfe = enviNfe.LoadFromFile(caminhoXml);

//             // Converter e salvar
//             var enviNfeEntity = await conversionService.ConvertAndSaveEnviNFeAsync(enviNfe);

//             logger.LogInformation("✅ XML importado com sucesso!");
//             logger.LogInformation("   - Lote: {Lote}", enviNfeEntity.IdLote);
//             logger.LogInformation("   - NFes importadas: {Count}", enviNfeEntity.NFes.Count);
//         }
//         catch (Exception ex)
//         {
//             logger.LogError(ex, "❌ Erro ao importar XML: {Caminho}", caminhoXml);
//             throw;
//         }
//     }
// }
