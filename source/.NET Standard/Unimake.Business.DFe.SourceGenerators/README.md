# Guia de Implementação - Source Generators para DTOs

## Estrutura Criada

### 1. Projeto Source Generator
- `Unimake.Business.DFe.SourceGenerators` - Contém o gerador automático de DTOs

### 2. Atributos Disponíveis
- `[GenerateDto()]` - Marca a classe para gerar DTOs
- `[ExcludeFromDto()]` - Exclui propriedades específicas
- `[DtoPropertyName()]` - Customiza nome da propriedade
- `[DtoRequired()]` - Marca propriedade como obrigatória

## Como Usar

### 1. Adicionar Referência ao Source Generator

No seu projeto principal (`Unimake.Business.DFe.csproj`), adicione:

```xml
<ItemGroup>
  <ProjectReference Include="..\Unimake.Business.DFe.SourceGenerators\Unimake.Business.DFe.SourceGenerators.csproj" 
                    OutputItemType="Analyzer" 
                    ReferenceOutputAssembly="false" />
</ItemGroup>
```

### 2. Marcar Classes para Geração

```csharp
using Unimake.Business.DFe.SourceGenerators.Attributes;

[GenerateDto(DtoType.IncluirRequest, DtoType.AtualizarRequest, DtoType.ConsultarRequest)]
public class EnviNFe : XMLBase
{
    [DtoRequired(DtoType.IncluirRequest)]
    public string Versao { get; set; }

    [DtoRequired(DtoType.IncluirRequest, DtoType.AtualizarRequest)]
    public string IdLote { get; set; }

    public SimNao IndSinc { get; set; }

    [DtoPropertyName("NotasFiscais")]
    public List<NFe> NFe { get; set; }

    // Esta propriedade será excluída dos DTOs de inclusão
    [ExcludeFromDto(DtoType.IncluirRequest)]
    public string DataCriacao { get; set; }
}
```

### 3. DTOs Gerados Automaticamente

O Source Generator criará automaticamente:

```csharp
// EnviNFeIncluirRequest.g.cs
namespace Unimake.Business.DFe.Xml.NFe.Dtos
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
```

## Vantagens da Abordagem

### 1. **Sincronização Automática**
- Quando você atualizar uma propriedade na classe XML, o DTO é atualizado automaticamente
- Sem necessidade de manutenção manual

### 2. **Flexibilidade**
- Controle granular sobre quais propriedades incluir/excluir
- Diferentes tipos de DTO para diferentes operações
- Customização de nomes e validações

### 3. **Performance**
- Geração em tempo de compilação (zero overhead em runtime)
- DTOs otimizados para JSON

### 4. **Type Safety**
- Todos os DTOs são fortemente tipados
- Detecção de erros em tempo de compilação

## Tipos de DTO Suportados

- `DtoType.Request` - DTO genérico de request
- `DtoType.Response` - DTO de response
- `DtoType.IncluirRequest` - Para operações de inclusão
- `DtoType.AtualizarRequest` - Para operações de atualização
- `DtoType.ExcluirRequest` - Para operações de exclusão
- `DtoType.ConsultarRequest` - Para operações de consulta

## Configuração Avançada

### Namespace Customizado
```csharp
[GenerateDto(DtoType.Request, CustomNamespace = "MeuProjeto.ApiModels")]
public class MinhaClasse { }
```

### Sufixo Customizado
```csharp
[GenerateDto(DtoType.Request, CustomSuffix = "Model")]
public class MinhaClasse { }
// Gera: MinhaClasseModel
```

## Exemplo de Uso em API

```csharp
[ApiController]
[Route("api/[controller]")]
public class NFController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> IncluirNF([FromBody] EnviNFeIncluirRequest request)
    {
        // Converter DTO para classe XML
        var enviNFe = new EnviNFe
        {
            Versao = request.Versao,
            IdLote = request.IdLote,
            IndSinc = request.IndSinc,
            NFe = request.NotasFiscais
        };

        // Processar...
        
        return Ok();
    }
}
```

## Próximos Passos

1. Compile o projeto para gerar os DTOs
2. Configure os atributos nas suas classes XML conforme necessário
3. Use os DTOs gerados em suas APIs
4. Configure mapeamentos automáticos (AutoMapper) se desejar

## Alternativas Consideradas

### 1. **Reflection em Runtime**
- ❌ Impacto na performance
- ❌ Menos type safety

### 2. **T4 Templates**
- ❌ Mais complexo de manter
- ❌ Menos integrado com o build

### 3. **AutoMapper com Profiles**
- ✅ Boa para conversões
- ❌ Não gera os DTOs automaticamente

### 4. **Manual**
- ❌ Propenso a erros
- ❌ Manutenção trabalhosa

## Conclusão

O Source Generator é a melhor abordagem para seu caso porque:
- **Automação completa** - Você só precisa marcar as classes
- **Flexibilidade total** - Controle sobre cada propriedade
- **Performance** - Zero overhead em runtime
- **Maintibilidade** - Sincronização automática com mudanças
- **Integração** - Funciona perfeitamente com o ecossistema .NET
