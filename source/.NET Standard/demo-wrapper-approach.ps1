# Script para demonstrar o uso das classes wrapper

Write-Host "=== Demonstração: Source Generators com Classes Wrapper ===" -ForegroundColor Green

Write-Host ""
Write-Host "🎯 PROBLEMA RESOLVIDO:" -ForegroundColor Yellow
Write-Host "✅ Não modificamos o projeto base Unimake.Business.DFe" -ForegroundColor Green
Write-Host "✅ Criamos classes wrapper que herdam das originais" -ForegroundColor Green  
Write-Host "✅ Configuramos DTOs via atributos nas classes wrapper" -ForegroundColor Green
Write-Host "✅ DTOs são gerados automaticamente em tempo de compilação" -ForegroundColor Green

Write-Host ""
Write-Host "📁 ESTRUTURA CRIADA:" -ForegroundColor Cyan
Write-Host "source/.NET Standard/" -ForegroundColor White
Write-Host "├── Unimake.Business.DFe                    # ❌ PROJETO BASE (NÃO MODIFICADO)" -ForegroundColor Gray
Write-Host "├── Unimake.Business.DFe.SourceGenerators   # ⚙️  SOURCE GENERATOR" -ForegroundColor Yellow
Write-Host "└── Unimake.Business.DFe.SourceGenerators.Wrappers # 🔧 CLASSES WRAPPER" -ForegroundColor Green

Write-Host ""
Write-Host "🔧 COMO FUNCIONA:" -ForegroundColor Cyan

Write-Host ""
Write-Host "1️⃣  CLASSE WRAPPER:" -ForegroundColor Yellow
Write-Host @"
[SourceGeneratorConfig(typeof(EnviNFe), DtoType.IncluirRequest, DtoType.AtualizarRequest)]
[PropertyConfig("Versao", RequiredForTypes = new[] { DtoType.IncluirRequest })]
[PropertyConfig("IdLote", RequiredForTypes = new[] { DtoType.IncluirRequest, DtoType.AtualizarRequest })]
[PropertyConfig("NFe", CustomName = "NotasFiscais")]
public class EnviNFeWrapper : EnviNFe
{
    // Esta classe só serve para configuração
    // Herda tudo da classe original
}
"@ -ForegroundColor Gray

Write-Host ""
Write-Host "2️⃣  DTO GERADO AUTOMATICAMENTE:" -ForegroundColor Yellow
Write-Host @"
// EnviNFeIncluirRequest.g.cs (GERADO AUTOMATICAMENTE)
namespace Unimake.Business.DFe.Dtos.NFe
{
    public partial class EnviNFeIncluirRequest
    {
        [Required]
        [JsonPropertyName("versao")]
        public string Versao { get; set; }

        [Required]
        [JsonPropertyName("idLote")]
        public string IdLote { get; set; }

        [JsonPropertyName("indSinc")]
        public SimNao IndSinc { get; set; }

        [JsonPropertyName("notasFiscais")]
        public List<NFe> NotasFiscais { get; set; }
    }
}
"@ -ForegroundColor Gray

Write-Host ""
Write-Host "3️⃣  USO EM API:" -ForegroundColor Yellow
Write-Host @"
[HttpPost]
public async Task<IActionResult> IncluirNFe([FromBody] EnviNFeIncluirRequest request)
{
    // Converter DTO para classe original (SEM MODIFICAÇÕES)
    var enviNFe = new EnviNFe
    {
        Versao = request.Versao,
        IdLote = request.IdLote,
        IndSinc = request.IndSinc,
        NFe = request.NotasFiscais
    };

    // Usar normalmente as classes originais
    var xml = enviNFe.GerarXML();
    
    return Ok();
}
"@ -ForegroundColor Gray

Write-Host ""
Write-Host "🚀 VANTAGENS DESTA ABORDAGEM:" -ForegroundColor Cyan
Write-Host "✅ Projeto base intocado - pode atualizar normalmente" -ForegroundColor Green
Write-Host "✅ DTOs sempre sincronizados - qualquer mudança no base é refletida" -ForegroundColor Green
Write-Host "✅ Configuração flexível - controle granular por propriedade" -ForegroundColor Green
Write-Host "✅ Performance - zero overhead em runtime" -ForegroundColor Green
Write-Host "✅ Type Safety - herança mantém compatibilidade" -ForegroundColor Green
Write-Host "✅ Isolamento - configurações ficam em projeto separado" -ForegroundColor Green

Write-Host ""
Write-Host "📋 PRÓXIMOS PASSOS:" -ForegroundColor Cyan
Write-Host "1. Compile o projeto Wrappers para gerar os DTOs:" -ForegroundColor White
Write-Host "   dotnet build Unimake.Business.DFe.SourceGenerators.Wrappers" -ForegroundColor Gray

Write-Host ""
Write-Host "2. Verifique os DTOs gerados em:" -ForegroundColor White
Write-Host "   obj/Debug/netstandard2.0/generated/" -ForegroundColor Gray

Write-Host ""
Write-Host "3. Use os DTOs em suas APIs:" -ForegroundColor White
Write-Host "   - EnviNFeIncluirRequest" -ForegroundColor Gray
Write-Host "   - EnviNFeAtualizarRequest" -ForegroundColor Gray
Write-Host "   - EnviNFeConsultarRequest" -ForegroundColor Gray

Write-Host ""
Write-Host "4. Configure mais wrappers conforme necessário:" -ForegroundColor White
Write-Host "   - NFeWrapper para NFe individual" -ForegroundColor Gray
Write-Host "   - InfNFeWrapper para informações da NFe" -ForegroundColor Gray
Write-Host "   - IdeWrapper para identificação" -ForegroundColor Gray

Write-Host ""
Write-Host "💡 DICA:" -ForegroundColor Yellow
Write-Host "Se precisar de DTOs para outras classes, apenas:" -ForegroundColor White
Write-Host "1. Crie uma classe wrapper herdando da original" -ForegroundColor Gray
Write-Host "2. Adicione os atributos de configuração" -ForegroundColor Gray
Write-Host "3. Compile e use os DTOs gerados" -ForegroundColor Gray

Write-Host ""
Write-Host "=== SOLUÇÃO IMPLEMENTADA COM SUCESSO! ===" -ForegroundColor Green
